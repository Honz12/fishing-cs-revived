using fishing_cs_revived.src.Ui;
using fishing_cs_revived.src.Data;

namespace fishing_cs_revived.src
{
    public enum GameState
    {
        BootScreen, MainMenu, Shop, Catching, Inventory, Advancements, Catalog
    }

    public class PlayerData
    {
        public uint Money = 0;
        public ushort RodLevel = 0;
        public byte InventorySize = 0;
        public byte HouseLevel = 0;
        public byte LocationUpgrade = 0;

        public uint FishCaughtCount = 0;
        public ulong TotalMoneyEarned = 0;

        public List<Fish> Inventory = new();

        public GameState GameState = GameState.BootScreen;

        public List<Advancement> Advancements = [];
        public List<string> UnlockedAdvancementIds = [];

        public List<int> UnlockedFishIds = [];

        public FishLocation CurrentLocation = FishLocation.Lake;
    }

    public class Program
    {
        // Constants

        public const string TITLE_COLOR = "\x1b[92m";

        const char LOWER_HALF_CHAR = '▄';
        const char UPPER_HALF_CHAR = '▀';
        const int CATCHING_UI_WIDTH = 100;
        const int JUMP_VEL_MIN = 4;
        const int JUMP_VEL_MAX = 6;

        // Variables

        public static Random Rng = new();

        public static PlayerData data = new();
        private static int catchingPos = 0;
        private static uint gameTicks = 0;
        private static int successfullyCatchingTicks = 0;
        private static int catchingCenterSize = 0;
        private static Fish? catchingFish = null;
        private static int catchingFishId = 0;
        private static uint requiredCatchingTicks = 0;
        private static bool currentlyCatching = false;
        private static int catchingOffset = 0;
        private static int catchingVel = 0;
        private static Image shopSeller = new("characters", "civil0.img");
        private static bool storySkipped = false;

        public static bool audioEnabled = true;

        // Helper functions

        public static string RepeatString(string s, int count) => string.Concat(Enumerable.Repeat(s, Math.Max(0, count)));

        public static ConsoleKey? ReadKeyNoBlock()
        {
            ConsoleKey? input = null;

            if (Console.KeyAvailable)
            {
                input = Console.ReadKey(true).Key;
            }

            return input;
        }

        public static bool AskYesNo()
        {
            ConsoleKey consoleKey = Console.ReadKey(true).Key;
            while (!(consoleKey == ConsoleKey.A || consoleKey == ConsoleKey.N || consoleKey == ConsoleKey.Y))
            {
                consoleKey = Console.ReadKey(true).Key;
            }
            return consoleKey == ConsoleKey.Y || consoleKey == ConsoleKey.A;
        }

        public static double GetMoneyMultiplier()
        {
            return data.HouseLevel switch
            {
                0 => 1.0,
                1 => 1.1,
                2 => 1.3,
                3 => 1.5,
                4 => 2.0,
                _ => throw new NotImplementedException()
            };
        }

        /// <summary>
        /// Gets the translated text with ANSI formatting.
        /// </summary>
        /// <param name="r">The rarity to be converted.</param>
        /// <returns>The translated and colored string.</returns>
        /// <exception cref="NotImplementedException">Fatal error.</exception>
        public static string GetTransRarity(FishRarity r) => r switch
        {
            FishRarity.Common => $"\x1b[37m{GetTransRarityNoColor(r)}\x1b[0m",
            FishRarity.Rare => $"\x1b[30;102m{GetTransRarityNoColor(r)}\x1b[0m",
            FishRarity.Epic => $"\x1b[30;105m{GetTransRarityNoColor(r)}\x1b[0m",
            FishRarity.Mythic => $"\x1b[30;101m{GetTransRarityNoColor(r)}\x1b[0m",
            FishRarity.Special => $"\x1b[30;103m{GetTransRarityNoColor(r)}\x1b[0m",
            _ => throw new NotImplementedException()
        };

        /// <summary>
        /// Gets the translated text without color formatting.
        /// </summary>
        /// <param name="r">The rarity to be converted.</param>
        /// <returns>The translated string.</returns>
        /// <exception cref="NotImplementedException">Fatal error.</exception>
        public static string GetTransRarityNoColor(FishRarity r) => r switch
        {
            FishRarity.Common => "Běžná",
            FishRarity.Rare => " Neobyčejná ",
            FishRarity.Epic => " Epická ",
            FishRarity.Mythic => " Mytická ",
            FishRarity.Special => " Speciální ",
            _ => throw new NotImplementedException()
        };

        public static string GetTransLocation(FishLocation l) => l switch
        {
            FishLocation.Lake => "Sladká voda",
            FishLocation.Sea => "Moře",
            FishLocation.DeepSea => "Hluboké moře",
            _ => throw new NotImplementedException()
        };

        // Lookup table mapping indices 0–15 directly to their RGB values
        private static readonly (byte R, byte G, byte B)[] ColorPalette = new (byte, byte, byte)[]
        {
            (0x00, 0x00, 0x00), // 0: Black
            (0x80, 0x00, 0x00), // 1: Red
            (0x00, 0x80, 0x00), // 2: Green
            (0x80, 0x80, 0x00), // 3: Yellow
            (0x00, 0x00, 0x80), // 4: Blue
            (0x80, 0x00, 0x80), // 5: Purple
            (0x00, 0x80, 0x80), // 6: Cyan
            (0xC0, 0xC0, 0xC0), // 7: Dim White
            (0x80, 0x80, 0x80), // 8: Gray
            (0xFF, 0x00, 0x00), // 9: Light Red
            (0x00, 0xFF, 0x00), // A: Light Green
            (0xFF, 0xFF, 0x00), // B: Light Yellow
            (0x00, 0x00, 0xFF), // C: Light Blue
            (0xFF, 0x00, 0xFF), // D: Light Purple
            (0x00, 0xFF, 0xFF), // E: Light Cyan
            (0xFF, 0xFF, 0xFF)  // F: White
        };

        /// <summary>
        /// Generates a two-pixel ansi-escaped character,
        /// all 0 values will be transparent.
        /// </summary>
        /// <param name="upper">The upper pixel</param>
        /// <param name="lower">The lower pixel</param>
        /// <returns>The ansi escaped string.</returns>
        public static string GetAnsiChar(byte upper, byte lower)
        {
            var (fgR, fgG, fgB) = ColorPalette[lower];
            var (bgR, bgG, bgB) = ColorPalette[upper];

            string fg = $"{fgR};{fgG};{fgB}m";
            string bg = $"{bgR};{bgG};{bgB}m";

            if (upper == 0 && lower == 0)
            {
                return "\x1b[0m ";
            }
            if (upper == 0)
            {
                return $"\x1b[0m\x1b[38;2;{fg}{LOWER_HALF_CHAR}";
            }
            if (lower == 0)
            {
                return $"\x1b[0m\x1b[38;2;{bg}{UPPER_HALF_CHAR}";
            }
            return $"\x1b[0m\x1b[38;2;{fg}\x1b[48;2;{bg}{LOWER_HALF_CHAR}";
        }

        /// <summary>
        /// Displays a 16x16 image,
        /// with a possible text on the side,
        /// which can be styled.
        /// </summary>
        /// <param name="image">The image to be displayed.</param>
        /// <param name="text">The text to be displayed.</param>
        /// <param name="base_color">The styling of the text, in ansi escape sequences.</param>
        public static void DisplayImage(Image image, string text = "", string base_color = "")
        {
            int textPointer = 0;

            for (int y = 0; y < 16; y += 2)
            {
                string line = "";

                for (int x = 0; x < 16; x++)
                {
                    byte upper = image.colors[x, y];
                    byte lower = image.colors[x, y + 1];

                    line += GetAnsiChar(upper, lower);
                }

                string additionalData = "";

                while (textPointer < text.Length)
                {
                    if (text[textPointer] == '\r')
                    {
                        textPointer++;
                        continue;
                    }
                    if (text[textPointer] == '\n')
                    {
                        textPointer++;
                        break;
                    }
                    additionalData += text[textPointer];
                    textPointer++;
                }

                Console.WriteLine(line + "\x1b[0m " + base_color + additionalData + "\x1b[0m");
            }
        }

        /// <summary>
        /// Displays multiple images in a row,
        /// with one character gaps between them.
        /// Images that failed to load are skipped.
        /// </summary>
        /// <param name="images">The images to be displayed.</param>
        public static void DisplayMultipleImages(Image[] images)
        {
            for (int y = 0; y < 16; y += 2)
            {
                string line = "";

                for (int i = 0; i < images.Length; i++)
                {
                    if (images[i].Failed)
                        continue;

                    for (int x = 0; x < 16; x++)
                    {
                        byte upper = images[i].colors[x, y];
                        byte lower = images[i].colors[x, y + 1];

                        line += GetAnsiChar(upper, lower);
                    }
                    line += "\x1b[0m ";
                }

                Console.WriteLine(line);
            }
        }

        /// <summary>
        /// Get the maximum size of the player's inventory,
        /// according to the boat level.
        /// </summary>
        /// <returns>The maximal count of items in player's inventory.</returns>
        public static int GetMaxFishInInventory()
        {
            switch (data.InventorySize)
            {
                case 0: return 3;
                case 1: return 5;
                case 2: return 7;
                case 3: return 10;
                case 4: return 15;
            }
            return 3;
        }

        private static void SayBoy(string msg)
        {
            Console.WriteLine($"{TITLE_COLOR}[Ty]\x1b[0m {msg}");
            if (Console.ReadKey(true).Key == ConsoleKey.Escape) storySkipped = true;
        }

        private static void SayDad(string msg)
        {
            Console.WriteLine($"{TITLE_COLOR}[Táta]\x1b[0m {msg}");
            if (Console.ReadKey(true).Key == ConsoleKey.Escape) storySkipped = true;
        }

        private static void SayStory(string msg)
        {
            Console.WriteLine($"{TITLE_COLOR}* {msg}");
            if (Console.ReadKey(true).Key == ConsoleKey.Escape) storySkipped = true;
        }

        /// <summary>
        /// Pretty self explanatory now, isn't it?
        /// </summary>
        public static void DisplayTragicBackstory()
        {
            Console.Clear();
            Console.WriteLine("Příběh, jakákoliv klávesa pro pokračování, ESC pro přeskočení:\n");

            SayBoy("Tati, chtěl bych se stát rybářem...");
            if (storySkipped) return;

            SayDad("Rybaření je pitomost! Nikdy se tím neuživíš!");
            if (storySkipped) return;

            SayDad("Vypadni odsud!");
            if (storySkipped) return;

            SayStory("Táta tě vykopnul z baráku.");
            if (storySkipped) return;

            SayStory("Máš v kapse jen 67 korun.");
            if (storySkipped) return;

            SayStory("Koupil sis za ně provázek na klacek co jsi našel v lese.");
            if (storySkipped) return;

            SayStory("Teď jdi a ukaž tátovi, že rybaření má smysl - tím že budeš nejbohatší rybář na světě!");
            if (storySkipped) return;

            SayBoy("A teď...");
            if (storySkipped) return;


        }

        /// <summary>
        /// Pretty self explanatory now, isn't it?
        /// </summary>
        public static void DisplayCompletionStory()
        {
            Console.Clear();
            Console.WriteLine("Příběh, jakákoliv klávesa pro pokračování:\n");

            Sound.PlayAudioFile("startCall.wav");

            SayStory("Najednou ti zavolá táta.");

            SayDad("Ahoj, vím že jsme spolu dlouho nemluvili, ale slyšel jsem, že jsi rozjel vlastní podnik a koupil sis vilu.");

            SayDad("Je to pravda?");

            SayStory("V tu chvíli si vzpomeneš, jak to celé začalo...");

            Sound.PlayAudioFile("endCall.wav");

            SayBoy("Položíš telefon bez odpovědi, usměješ se a řekneš si:");

            SayBoy("A teď už vím, kde ty ryby jsou!");
        }

        /// <summary>
        /// Display game credits
        /// </summary>
        public static void DisplayCredits()
        {
            Console.Clear();

            string credits = @" === Kde Jsou Ryby ===
    Vytvořeno v C# pro .NET 10, projekt licencován pod MIT licencí.

    Vývojáři:
    - Honz12               : Engine kód
    - mistrmatej           : Game design
    - sebastianjecny-green : Grafika
    ";

            string vyvojariText = @"
 __   __ __          _   __ _ _    
 \ \ / //_/__ _____ (_)_/_/ \ˇ/(_) 
  \ V / || \ V / _ \| / _` | '_| | 
   \_/ \_, |\_/\___// \__,_|_| |_| 
        |__/       |__/            
    ";

            string mistrmatejText = @$"
    {TITLE_COLOR}mistrmatej
    - Udělal Obchod.
    - Balancoval hru.
    - Vytvořil všechny data ryb a odznaků.
    ";
            string honz12Text = @$"
    {TITLE_COLOR}Honz12
    - Udělal engine hry.
    - Přispěl pár obrázky pro grafiku.
    - Obnovil projekt fishing-cs-hackathon.
    ";
            string sebastianjecnyText = @$"
    {TITLE_COLOR}sebastianjecny-green
    - Udělal všechnu grafiku původní hry.
    - Vytvořil nápady pro několik systémů.
    ";

            DisplayImage(new Image("ui", "csLogo.img"), credits);

            Console.WriteLine(TITLE_COLOR + vyvojariText + "\x1b[0m");

            DisplayImage(
                new Image("ui", "iconHonz12.img"),
                honz12Text
            );
            DisplayImage(
                new Image("ui", "iconMistrmatej.img"),
                mistrmatejText
            );
            DisplayImage(
                new Image("ui", "iconSebastianjecny.img"),
                sebastianjecnyText
            );

            Console.WriteLine($"{TITLE_COLOR}https://github.com/Honz12/fishing-cs-revived\x1b[0m - Zdrojový kód hry");
            Console.WriteLine("Jakákoliv klávesa pro pokračování ...");

            Sound.PlayAudioFile("soundTest.wav");

            Console.ReadKey(true);
        }

        public static void CheckForNewAdvancements()
        {
            Advancement? newAdvancement = AdvancementProcessor.CheckForNewAdvancements(data);
            while (newAdvancement != null)
            {
                data.Advancements.Add(newAdvancement);
                Console.WriteLine();
                Console.WriteLine($"{TITLE_COLOR}Nový Odznak odemčen: {newAdvancement.Name} ({newAdvancement.Description})\x1b[0m");
                Console.WriteLine($"Stikněte libovolnou klávesu ...");
                Console.ReadKey(true);
                newAdvancement = AdvancementProcessor.CheckForNewAdvancements(data);
            }
        }

        /// <summary>
        /// The main function,
        /// what would you expect?
        /// </summary>
        public static void Main()
        {
            DisplayCredits();

            if (SaveGameHandler.LoadGame(data, false))
            {
                Console.WriteLine("Na tomto zařízení už máš uloženou hru, načíst? (A/n)");
                if (!AskYesNo())
                {
                    data = new();
                    DisplayTragicBackstory();
                }
            }
            else
            {
                DisplayTragicBackstory();
            }

            shopSeller = new Image("characters", (new string[]
            {
                "civil0.img", "civil1.img", "civil2.img",
                "kapitan.img", "namornik.img", "pepek.img",
                "rybar0.img", "rybar1.img", "rybar2.img", "rybar3.img"
            })[Rng.Next(0, 10)]);

            Console.CursorVisible = false;

            while (true)
            {
                switch (data.GameState)
                {
                    case GameState.BootScreen:
                        {
                            Console.Clear();

                            string title =
    "\n" +
    @"     __ __    __            __                    ____        __             ___  __ " + '\n' +
    @"    / //_/___/ /__         / /________  __  __   / __ \__  __/ /_  __  __   /__ \/ / " + '\n' +
    @"   / ,< / __  / _ \   __  / / ___/ __ \/ / / /  / /_/ / / / / __ \/ / / /    / _/ /  " + '\n' +
    @"  / /| / /_/ /  __/  / /_/ (__  ) /_/ / /_/ /  / _, _/ /_/ / /_/ / /_/ /    /_//_/   " + '\n' +
    @" /_/ |_\__,_/\___/   \____/____/\____/\__,_/  /_/ |_|\__, /_.___/\__, /    (_)(_)    " + '\n' +
    @"                                                    /____/      /____/               " + "\n";
                            DisplayImage(new Image("ship", "lod3.img"), title, TITLE_COLOR); // Display the images/ship/lod3.img image (the icon of the game), with the title.

                            Console.WriteLine("Jakákoliv klávesa pro pokračování ...");

                            Console.ReadKey(true);
                            data.GameState = GameState.MainMenu;
                        }
                        break;
                    case GameState.MainMenu:
                        {
                            Console.Clear();
                            MainMenu.DisplayMenu();
                            ConsoleKey key = Console.ReadKey(true).Key;
                            switch (key)
                            {
                                case ConsoleKey.UpArrow:
                                    MainMenu.UiButtonMenuUp();
                                    break;
                                case ConsoleKey.DownArrow:
                                    MainMenu.UiButtonMenuDown();
                                    break;
                                case ConsoleKey.Spacebar:
                                case ConsoleKey.Enter:
                                    MainMenu.EnterOption(data);
                                    break;
                                case ConsoleKey.F1:
                                    CommandProc.Enter(data);
                                    break;
                            }
                        }
                        break;
                    case GameState.Shop:
                        {
                            Console.Clear();
                            Shop.DisplayShop(data, shopSeller);
                            ConsoleKey key = Console.ReadKey(true).Key;
                            switch (key)
                            {
                                case ConsoleKey.UpArrow:
                                    Shop.ShopButtonMenuUp();
                                    break;
                                case ConsoleKey.DownArrow:
                                    Shop.ShopButtonMenuDown();
                                    break;
                                case ConsoleKey.Spacebar:
                                case ConsoleKey.Enter:
                                    Shop.EnterOption(data);
                                    CheckForNewAdvancements();
                                    break;
                                case ConsoleKey.Escape:
                                    data.GameState = GameState.MainMenu;
                                    break;
                            }
                        }
                        break;
                    case GameState.Catching:
                        {
                            if (catchingFishId == -1)
                            {
                                Console.WriteLine("Nemáš dost dobrý prut, aby jsi tu mohl rybařit.");
                                Console.ReadKey(true);
                                data.GameState = GameState.MainMenu;
                                break;
                            }

                            if (data.Inventory.Count >= GetMaxFishInInventory()) // The capacity limit has been reached.
                            {
                                Console.WriteLine("Do tvého chladícího boxu se už nic nevejde.");
                                Console.ReadKey(true);
                                data.GameState = GameState.MainMenu;
                                break;
                            }

                            int sideBarWidth = CATCHING_UI_WIDTH - catchingCenterSize + catchingOffset;
                            int leftWidth = sideBarWidth / 2;

                            Console.Write("\x1b[H"); // ANSI HOME


                            string title =
        Program.TITLE_COLOR + @"  _____     _         _ _ " + "\x1b[0m\n" +
        Program.TITLE_COLOR + @" |_   _|_ _| |_  ___ (_) |" + "\x1b[0m\n" +
        Program.TITLE_COLOR + @"   | |/ _` | ' \/ -_)| |_|" + "\x1b[0m\n" +
        Program.TITLE_COLOR + @"   |_|\__,_|_||_\___|/ (_)" + "\x1b[0m\n" +
        Program.TITLE_COLOR + @"                   |__/   ";

                            Console.WriteLine(title);
                            Console.WriteLine($"Právě se nacházíš na {data.CurrentLocation}");
                            Console.WriteLine();

                            Console.WriteLine(
                                "Ryba je " + GetTransRarity((catchingFish ?? new Fish()).Rarity) // Display fish rarity.
                            );
                            Console.WriteLine();
                            for (int j = 0; j < 2; j++)
                            {
                                string line = "";
                                byte color = 0;

                                for (int i = 0; i < CATCHING_UI_WIDTH; i++)
                                {
                                    byte desiredColor;

                                    if (leftWidth <= i && i < leftWidth + catchingCenterSize)
                                        desiredColor = 102;
                                    else
                                        desiredColor = 101;
                                    if (i == catchingPos)
                                        desiredColor = 0;

                                    if (desiredColor != color)
                                    {
                                        line += $"\x1b[{desiredColor}m";
                                        color = desiredColor;
                                    }
                                    line += ' ';
                                }

                                Console.WriteLine(line + "\x1b[0m"); // Write the catching bar.
                            }

                            Console.WriteLine();

                            int progress = (int)(((double)successfullyCatchingTicks) / ((double)requiredCatchingTicks) * CATCHING_UI_WIDTH); // Calculate progress

                            Console.WriteLine("\x1b[0;106m" + RepeatString(" ", Math.Max(0, progress)) + "\x1b[0m" + RepeatString(" ", Math.Max(0, CATCHING_UI_WIDTH - progress))); // Write the progress bar.

                            if (!currentlyCatching) // When the game just started, wait until a key is pressed.
                            {
                                Console.WriteLine("Zmáčkni klávesu pro start ....");
                                Console.ReadKey(true);
                                currentlyCatching = true;
                            }
                            else
                                Console.WriteLine("                              "); // Overwrite the previous text

                            ConsoleKey? input = ReadKeyNoBlock(); // Non blocking input, allowing the game to run at 100 FPS.

                            if (input != null)
                            {
                                if (input == ConsoleKey.Escape)
                                {
                                    data.GameState = GameState.MainMenu;
                                    currentlyCatching = false;
                                }
                                else // We "jump" the position.
                                {
                                    catchingPos += Rng.Next(JUMP_VEL_MIN, JUMP_VEL_MAX);
                                }
                            }

                            if (gameTicks % 5 == 0) // 1 per 0.05 seconds
                                if (leftWidth <= catchingPos && catchingPos < leftWidth + catchingCenterSize)
                                {
                                    if (gameTicks % 10 == 0) // 1 per 0.1 seconds
                                        successfullyCatchingTicks++;
                                }
                                else
                                    successfullyCatchingTicks--;

                            if (gameTicks % (catchingFish ?? new Fish()).Rarity switch // Get the moving speed based on the rarity
                            {
                                FishRarity.Common => 50,
                                FishRarity.Rare => 15,
                                FishRarity.Epic => 10,
                                FishRarity.Mythic => 5,
                                FishRarity.Special => 3,
                                _ => 0
                            } == 0)
                                catchingOffset += catchingVel;  // If passed, move the green part
                            if (catchingOffset < -20) // If outside of set bounds, reverse direction
                                catchingVel = 1;
                            if (catchingOffset > 20)
                                catchingVel = -1;

                            if (successfullyCatchingTicks < 0)
                            {
                                Console.WriteLine("Ryba uplavala!");
                                DisplayImage((catchingFish ?? new Fish()).Image, (catchingFish ?? new Fish()).GetFormatedData());
                                currentlyCatching = false;
                                Console.WriteLine("Jakákoliv klávesa pro pokračování ...");
                                Sound.PlayAudioFile("catchFailed.wav");
                                Console.ReadKey();
                                data.GameState = GameState.MainMenu;
                            }
                            else if (successfullyCatchingTicks >= requiredCatchingTicks) // If player successfully reached the goal, we give the win condition
                            {
                                CatalogUi.UnlockFish(catchingFishId);
                                data.FishCaughtCount++;
                                Console.WriteLine("Chytil jsi:");
                                DisplayImage((catchingFish ?? new Fish()).Image, (catchingFish ?? new Fish()).GetFormatedData());
                                Console.Write("Ponechat? (A/n)");
                                Sound.PlayAudioFile("catchSuccessfull.wav");
                                if (AskYesNo())
                                {
                                    data.Inventory.Add(catchingFish ?? new Fish());
                                }
                                Console.WriteLine();
                                CheckForNewAdvancements();
                                data.GameState = GameState.MainMenu;
                                currentlyCatching = false;
                            }

                            gameTicks++;

                            if (gameTicks % 5 == 0)
                                catchingPos--;

                            Thread.Sleep(10);
                        }
                        break;
                    case GameState.Inventory:
                        {
                            Console.Clear();
                            InventoryUi.DisplayMenu(data);
                            ConsoleKey key = Console.ReadKey(true).Key;
                            Console.Clear();
                            switch (key)
                            {
                                case ConsoleKey.UpArrow:
                                    InventoryUi.UiButtonMenuUp(data);
                                    break;
                                case ConsoleKey.DownArrow:
                                    InventoryUi.UiButtonMenuDown(data);
                                    break;
                                case ConsoleKey.S:
                                    InventoryUi.SellOption(data);
                                    CheckForNewAdvancements();
                                    break;
                                case ConsoleKey.Escape:
                                    data.GameState = GameState.MainMenu;
                                    break;
                            }
                        }
                        break;
                    case GameState.Advancements:
                        {
                            Console.Clear();

                            AdvancementUi.Display();
                            switch (Console.ReadKey(true).Key)
                            {
                                case ConsoleKey.UpArrow:
                                    AdvancementUi.UiButtonMenuUp();
                                    break;
                                case ConsoleKey.DownArrow:
                                    AdvancementUi.UiButtonMenuDown();
                                    break;
                                case ConsoleKey.Escape:
                                    data.GameState = GameState.MainMenu;
                                    break;
                            }
                        }
                        break;
                    case GameState.Catalog:
                        {
                            Console.Clear();
                            CatalogUi.ShowCatalog();
                            Console.WriteLine("\nJakákoliv klávesa pro návrat ...");
                            Console.ReadKey(true);
                            data.GameState = GameState.MainMenu;
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// The initialization function when entering <code>GameState.Catching</code>
        /// </summary>
        public static void CatchingInit()
        {
            catchingPos = CATCHING_UI_WIDTH / 2;
            gameTicks = 0;
            successfullyCatchingTicks = 0;
            catchingCenterSize = Rng.Next(10, 20);
            requiredCatchingTicks = (uint)Rng.Next(20, 50);
            catchingFishId = TFishFinder.FindRandomFish(data.RodLevel, data.CurrentLocation);
            if (catchingFishId != -1)
                catchingFish = new Fish(catchingFishId);
            else
                return;
            catchingOffset = 0;
            if ((int)catchingFish.Rarity >= (int)FishRarity.Rare)
            {
                catchingVel = Rng.Next(0, 2) * 2 - 1;
            }
            else
            {
                catchingVel = 0;
            }
        }
    }
}
