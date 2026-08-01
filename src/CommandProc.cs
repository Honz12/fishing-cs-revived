class CommandProc
{
    private static PlayerData data;

    private static bool running = false;

    private static string helpString = @"Kde Jsou Ryby!? Debug Command Interface

Commands:
    quit or exit                           Exits the Kde Jsou Ryby!? Debug Command Interface.
    help                            Shows the Kde Jsou Ryby!? Debug Command Interface help text.
    money <amount>                  Sets the money of the player.
    upgrade rod <level>             Sets the upgrade level of the fishing rod.
    upgrade ship <level>            Sets the upgrade level of the ship.
    upgrade house <level>           Sets the upgrade level of the house.
    fish list                       Lists all available fish.
    fish add <fish_id>              Adds a fish to the player's inventory.
    advrefresh                      Checks for new advancements and adds them to the player's data if any are found.
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
                                data.Money = (uint) v;
                        }
                        break;
                    case "fish":
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
                                    data.RodLevel = (ushort) v;
                            }

                            else if (parts[1] == "ship")
                            {
                                if (success)
                                    data.InventorySize = (byte) v;
                            }
                            
                            else if (parts[1] == "house")
                            {
                                if (success)
                                    data.HouseLevel = (byte) v;
                            }
                        }
                        break;
                    case "fish":
                        if (parts[1] == "add")
                        {
                            bool success = int.TryParse(parts[2], out int v);

                            if (success)
                                if (v >= 0 && v < FishData.fishes.Length)
                                    Program.data.Inventory.Add(new Fish(FishData.fishes[v]));
                        }
                        break;
                }
                break;
        }
    }
}