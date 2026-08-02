using fishing_cs_revived.src.Data;

namespace fishing_cs_revived.src.Ui
{
    public class CommandProc
    {
        private static PlayerData? data;

        private static bool running = false;

        private static string helpString = @"Kde Jsou Ryby!? Debug Command Interface

    Commands:
        quit or exit                    Exits the Kde Jsou Ryby!? Debug Command Interface.
        help                            Shows the Kde Jsou Ryby!? Debug Command Interface help text.
        money <amount>                  Sets the money of the player.
        upgrade rod <level>             Sets the upgrade level of the fishing rod.
        upgrade ship <level>            Sets the upgrade level of the shi°+p.
        upgrade house <level>           Sets the upgrade level of the house.
        fish list                       Lists all available fish.
        fish add <fish_id>              Adds a fish to the player's inventory.
        advrefresh                      Checks for new advancements and adds them to the player's data if any are found.
        sound                           Toggles sound on or off.
        catalog add <fish_id>           Adds a fish id to the catalog.
        catalog add all                 Adds all the fish to the catalog.
        catalog rem <fish_id>           Removes a fish id from the catalog.
        catalog rem all                 Removes all the fish from the catalog.
        save                            Saves the game to the save file.
        load                            Loads the game from the save file.
        shut                            Shuts off the game with no saving.
";

        public static void Enter(PlayerData playerData)
        {
            Console.CursorVisible = true;

            data = playerData;

            Console.Clear();
            Console.WriteLine("Welcome to Kde Jsou Ryby!? Debug Command Interface");

            running = true;
            Loop();
        }

        private static void Loop()
        {
            while (running)
            {
                Console.Write(">>> ");

                string? input = Console.ReadLine();

                if (input != null)
                {
                    ProcessCommand(input);
                }
            }
        }

        private static void ProcessCommand(string cmd)
        {
            List<string> parts = new List<string>();
            string part = "";

            foreach (char c in cmd)
            {
                if (c == ' ')
                {
                    parts.Add(part);
                    part = "";
                }
                else
                {
                    part += c;
                }
            }
            parts.Add(part);
            part = "";

            switch (parts.Count)
            {
                case 1:
                    switch (parts[0])
                    {
                        case "quit":
                            Console.CursorVisible = false;
                            running = false;
                            break;
                        case "exit":
                            Console.CursorVisible = false;
                            running = false;
                            break;
                        case "help":
                            Console.Write(helpString);
                            break;
                        case "advrefresh":
                            Program.CheckForNewAdvancements();
                            Console.WriteLine("Advancements refreshed.");
                            break;
                        case "sound":
                            Program.audioEnabled = !Program.audioEnabled;
                            Console.WriteLine($"Sound is now {(Program.audioEnabled ? "enabled" : "disabled")}.");
                            break;
                        case "save":
                            if (SaveGameHandler.SaveGame(data!))
                                Console.WriteLine("Saved successfully.");
                            else
                                Console.WriteLine("Save failed.");
                            break;
                        case "load":
                            if (SaveGameHandler.LoadGame(data!))
                                Console.WriteLine("Loaded successfully.");
                            else
                                Console.WriteLine("Load failed.");
                            break;
                        case "shut":
                            Console.CursorVisible = true;
                            Environment.Exit(0);
                            break;
                    }
                    break;
                case 2:
                    switch (parts[0])
                    {
                        case "money":
                            {
                                bool success = int.TryParse(parts[1], out int v);

                                if (success)
                                    data!.Money = (uint) v;
                                    Console.WriteLine($"Money set to {data!.Money}.");
                            }
                            break;
                        case "fish":
                            {
                                if (parts[1] == "list")
                                {
                                    Console.WriteLine("Available fish:");
                                    Console.WriteLine(Program.TITLE_COLOR + "\x1b[1m" + "|  ID  |         NAME         |    RARITY    |    WEIGHT   | WEIGHT VAR |" + "\x1b[0m");
                                    for (int i = 0; i < FishData.fishes.Length; i++)
                                    {
                                        TFish fish = FishData.fishes[i];
                                        Console.WriteLine($"| {i, 4} | {fish.Name, 20} | {fish.Rarity, 12} | {fish.Weight, 8} kg | {fish.WeightVar, 7} kg |");
                                    }
                                }
                            }
                            break;
                    }
                    break;
                case 3:
                    switch (parts[0])
                    {
                        case "upgrade":
                            {
                                bool success = int.TryParse(parts[2], out int v);

                                if (parts[1] == "rod")
                                {
                                    if (success)
                                        data!.RodLevel = (ushort) v;
                                        Console.WriteLine($"Rod level set to {data!.RodLevel}.");
                                }

                                else if (parts[1] == "ship")
                                {
                                    if (success)
                                        data!.InventorySize = (byte) v;
                                        Console.WriteLine($"Ship level set to {data!.InventorySize}.");
                                }
                                
                                else if (parts[1] == "house")
                                {
                                    if (success)
                                        data!.HouseLevel = (byte) v;
                                        Console.WriteLine($"House level set to {data!.HouseLevel}.");
                                }
                            }
                            break;
                        case "fish":
                            {
                                if (parts[1] == "add")
                                {
                                    bool success = int.TryParse(parts[2], out int v);

                                    if (success)
                                        if (v >= 0 && v < FishData.fishes.Length)
                                        {
                                            Fish fish = new(v);
                                            data!.Inventory.Add(fish);
                                            Console.WriteLine("Added fish to inventory.");
                                            Program.DisplayImage(fish.Image, fish.GetFormatedData());
                                        }
                                }
                            }
                            break;
                        case "catalog":
                            {
                                if (parts[1] == "add")
                                {
                                    if (parts[2] == "all")
                                    {
                                        for (int i = 0; i < FishData.fishes.Length; i++)
                                        {
                                            CatalogUi.UnlockFish(i);
                                        }
                                        Console.WriteLine("Added all entries to catalog.");
                                        break;
                                    }
                                    bool success = int.TryParse(parts[2], out int v);

                                    if (success)
                                    {
                                        CatalogUi.UnlockFish(v);
                                        Console.WriteLine("Added an entry to catalog.");
                                    }
                                }
                                else if (parts[1] == "rem")
                                {
                                    
                                    if (parts[2] == "all")
                                    {
                                        for (int i = 0; i < FishData.fishes.Length; i++)
                                        {
                                            CatalogUi.UnUnlockFish(i);
                                        }
                                        Console.WriteLine("Removed all entries from catalog.");
                                        break;
                                    }
                                    bool success = int.TryParse(parts[2], out int v);

                                    if (success)
                                    {
                                        CatalogUi.UnUnlockFish(v);
                                        Console.WriteLine("Removed an entry from catalog.");
                                    }
                                }
                            }
                            break;
                    }
                    break;
            }
        }
    }
}
