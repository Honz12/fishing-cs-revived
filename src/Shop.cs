using System;

public class Shop
{
    public static int selected = 0;
    private const int TOTAL_OPTIONS = 4;

    // Helper methods for price calculation.
    public static uint GetRodUpgradeCost(ushort currentLevel) => (uint)((currentLevel + 1) * 250);
    public static uint GetInventoryUpgradeCost(byte currentSize) => (uint)((currentSize + 1) * 270);
    public static uint GetHouseUpgradeCost(byte houseLevel) => (uint)((houseLevel + 1) * 1000);

    public static void DisplayShop(PlayerData playerData, Image character)
    {
        string title = (
            "\n\n" +
            Program.TITLE_COLOR + @"   ___  _        _            _ " + "\x1b[0m\n" +
            Program.TITLE_COLOR + @"  / _ \| |__  __| |_  ___  __| |" + "\x1b[0m\n" +
            Program.TITLE_COLOR + @" | (_) | '_ \/ _| ' \/ _ \/ _` |" + "\x1b[0m\n" +
            Program.TITLE_COLOR + @"  \___/|_.__/\__|_||_\___/\__,_|" + "\x1b[0m\n");

        Console.WriteLine(title);
        Console.WriteLine($"Tvoje peníze: {Program.data.Money} korun");
        Console.WriteLine();

        string str = "\n\n";

        uint rodCost = GetRodUpgradeCost(Program.data.RodLevel);
        uint invCost = GetInventoryUpgradeCost(Program.data.InventorySize);
        uint houseCost = GetHouseUpgradeCost(Program.data.HouseLevel);

        // Option 0: Fishing rod upgrade.
        string rodOption;
        if (Program.data.RodLevel >= 10) // If can't buy more upgrades.
        {
            rodOption = "Vylepšit Prut - MAX ÚROVEŇ";
        }
        else
        {
            rodOption = $"Vylepšit Prut ({Program.data.RodLevel + 1} / 11) - Cena: {rodCost} korun";
        }
        str += (selected == 0 ? "> " : "  ") + rodOption + "\n";

        // Option 1: Boat upgrade.
        string invOption;
        if (Program.data.InventorySize >= 4) // If can't buy more upgrades.
        {
            invOption = "Vylepšit Loď - MAX ÚROVEŇ";
        }
        else
        {
            invOption = $"Vylepšit Loď ({Program.data.InventorySize + 1} / 5) - Cena: {invCost} korun";
        }
        str += (selected == 1 ? "> " : "  ") + invOption + "\n";

        // Option 2: House upgrade.
        string houseOption;
        if (Program.data.HouseLevel >= 4) // If can't buy more upgrades.
        {
            houseOption = "Vylepšit Obydlí - MAX ÚROVEŇ";
        }
        else
        {
            houseOption = $"Vylepšit Obydlí ({Program.data.HouseLevel + 1} / 5) - Cena: {houseCost} korun";
        }
        str += (selected == 2 ? "> " : "  ") + houseOption + "\n";

        // Option 3: Zpět do menu
        str += (selected == 3 ? "> " : "  ") + "Zpět do hlavního menu" + "\n";

        Program.DisplayImage(character, str);

        Console.WriteLine();

        Program.DisplayMultipleImages( // Display the fishing rod and the boat images.
            new Image[]
            {
                new("rod", $"prut{playerData.RodLevel}.txt"),
                new("ship", $"lod{playerData.InventorySize}.txt"),
                new("houses", $"dum{playerData.HouseLevel}.txt"),
            }
        );
    }

    /// <summary>
    /// Called when the user presses <code>ConsoleKey.DownArrow</code>
    /// </summary>
    public static void ShopButtonMenuDown()
    {
        selected = (selected + 1) % TOTAL_OPTIONS;
    }

    /// <summary>
    /// Called when the user presses <code>ConsoleKey.UpArrow</code>
    /// </summary>
    public static void ShopButtonMenuUp()
    {
        selected = (selected - 1 + TOTAL_OPTIONS) % TOTAL_OPTIONS;
    }

    /// <summary>
    /// Called when the user presses <code>ConsoleKey.Enter</code>
    /// </summary>
    /// <param name="playerData">Player's data</param>
    public static void EnterOption(PlayerData playerData)
    {
        switch (selected)
        {
            case 0: // Rod upgrade.
                if (playerData.RodLevel < 10)
                {
                    uint rodCost = GetRodUpgradeCost(playerData.RodLevel);
                    if (playerData.Money >= rodCost)
                    {
                        playerData.Money -= rodCost;
                        playerData.RodLevel++;
                    }
                }
                break;

            case 1: // Inventory upgrade.
                if (playerData.InventorySize < 4)
                {
                    uint invCost = GetInventoryUpgradeCost(playerData.InventorySize);
                    if (playerData.Money >= invCost)
                    {
                        playerData.Money -= invCost;
                        playerData.InventorySize++;
                    }
                }
                break;

            case 2: // House upgrade.
                if (playerData.HouseLevel < 4)
                {
                    uint houseCost = GetHouseUpgradeCost(playerData.HouseLevel);
                    if (playerData.Money >= houseCost)
                    {
                        playerData.Money -= houseCost;
                        playerData.HouseLevel++;

                        if (playerData.HouseLevel == 4)
                        {
                            Program.DisplayCompletionStory();
                        }
                    }
                }
                break;

            case 3: // Back To Menu
                playerData.GameState = GameState.MainMenu;
                break;
        }
    }
}
