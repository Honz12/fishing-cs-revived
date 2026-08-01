namespace fishing_cs_revived.src.Ui
{
    public class MainMenu
    {
        static int selected = 0;

        /// <summary>
        /// Display the UI.
        /// </summary>
        public static void DisplayMenu()
        {
            string title = (
            "\n\n" +
            Program.TITLE_COLOR + @"   _  _ _               __  __  __                " + "\x1b[0m\n" +
            Program.TITLE_COLOR + @"  | || | |__ ___ ___ _ /_/ |  \/  |___ _ _ _  _   " + "\x1b[0m\n" +
            Program.TITLE_COLOR + @"  | __ | / _` \ V / ' \| | | |\/| / -_) ' \ || |  " + "\x1b[0m\n" +
            Program.TITLE_COLOR + @"  |_||_|_\__,_|\_/|_||_|_| |_|  |_\___|_||_\_,_|  " + "\x1b[0m\n");

            Console.WriteLine(title);

            if (selected == 0)
                Console.WriteLine("> Jít chytat ryby");
            else
                Console.WriteLine("  Jít chytat ryby");

            if (selected == 1)
                Console.WriteLine("> Jít do Obchodu");
            else
                Console.WriteLine("  Jít do Obchodu");

            if (selected == 2)
                Console.WriteLine("> Chladící Box");
            else
                Console.WriteLine("  Chladící Box");

            if (selected == 3)
                Console.WriteLine("> Otevřít Odznaky");
            else
                Console.WriteLine("  Otevřít Odznaky");

            if (selected == 4)
                Console.WriteLine("> Otevřít Katalog");
            else
                Console.WriteLine("  Otevřít Katalog");

            if (selected == 5)
                Console.WriteLine("> Opustit Hru");
            else
                Console.WriteLine("  Opustit Hru");
        }

        /// <summary>
        /// Called when the user presses <code>ConsoleKey.DownArrow</code>
        /// </summary>
        public static void UiButtonMenuDown()
        {
            selected++;
            selected += 6;
            selected %= 6;
        }

        /// <summary>
        /// Called when the user presses <code>ConsoleKey.UpArrow</code>
        /// </summary>
        public static void UiButtonMenuUp()
        {
            selected--;
            selected += 6;
            selected %= 6;
        }

        /// <summary>
        /// Called when the user presses <code>ConsoleKey.Enter</code>
        /// </summary>
        /// <param name="playerData">Player's data</param>
        public static void EnterOption(PlayerData playerData)
        {
            Console.Clear();

            switch (selected)
            {
                case 0: playerData.GameState = GameState.Catching; Program.CatchingInit(); break;
                case 1: playerData.GameState = GameState.Shop; Shop.selected = 0; break;
                case 2: playerData.GameState = GameState.Inventory; InventoryUi.Selected = 0; break;
                case 3: playerData.GameState = GameState.Advancements; AdvancementUi.Selected = 0; break;
                case 4: playerData.GameState = GameState.Catalog; break;
                case 5: Console.CursorVisible = true; Environment.Exit(0); break;
            }
        }
    }
}
