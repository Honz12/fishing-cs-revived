namespace fishing_cs_revived.src.Ui
{
    public class Shop
    {
        public static int selected = 0;
        private const int TOTAL_OPTIONS = 5;

        // Helper methods for price calculation.
        public static uint GetRodUpgradeCost(ushort currentLevel) => (uint)((currentLevel + 1) * 250);
        public static uint GetInventoryUpgradeCost(byte currentSize) => (uint)((currentSize + 1) * 270);
        public static uint GetHouseUpgradeCost(byte houseLevel) => (uint)((houseLevel + 1) * 3000);
        public static uint GetLocationUpgradeCost(byte loc) => loc switch
        {
            0 => 5000,
            1 => 10000,
            2 => 15000,
            _ => throw new NotImplementedException()
        };
        public static byte GetLocationUpgradeBoatReq(byte loc) => loc switch
        {
            0 => 1,
            1 => 3,
            2 => 5,
            _ => throw new NotImplementedException()
        };

        public static void DisplayShop(PlayerData playerData, Image character)
        {
            string title = (
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
            uint locationCost = GetLocationUpgradeCost(playerData.LocationUpgrade);
            byte locationReq = GetLocationUpgradeBoatReq(playerData.LocationUpgrade);

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

            // Option 2: Location unlock.
            string locationOption;
            if (Program.data.LocationUpgrade >= 2) // If can't unlock more.
            {
                locationOption = "Odemknout Lokace - VŠECHNY ODEMČENY";
            }
            else
            {
                locationOption = $"Odemknout Lokace ({Program.data.LocationUpgrade + 1} / 3) - Cena: {locationCost} korun\n      Potřebná loď {locationReq + 1} nebo lepší.";
            }
            str += (selected == 3 ? "> " : "  ") + locationOption + "\n";

            // Option 3: Zpět do menu
            str += (selected == 4 ? "> " : "  ") + "Zpět do hlavního menu" + "\n";

            Program.DisplayImage(character, str);

            Console.WriteLine();

            Program.DisplayMultipleImages(
                [
                    new("rod", $"prut{playerData.RodLevel}.img"),
                    new("ship", $"lod{playerData.InventorySize}.img"),
                    new("houses", $"dum{playerData.HouseLevel}.img"),
                    new("locPasses", $"povol{playerData.LocationUpgrade}.img"),
                ]
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
                            Sound.PlayAudioFile("buy.wav");
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
                            Sound.PlayAudioFile("buy.wav");
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
                            Sound.PlayAudioFile("buy.wav");

                            if (playerData.HouseLevel == 4)
                            {
                                Program.DisplayCompletionStory();
                            }
                        }
                    }
                    break;

                case 3: // Location unlock.
                    if (playerData.InventorySize >= GetLocationUpgradeBoatReq(playerData.LocationUpgrade))
                    if (playerData.LocationUpgrade < 3)
                    {
                        uint locationCost = GetLocationUpgradeCost(playerData.LocationUpgrade);
                        if (playerData.Money >= locationCost)
                        {
                            playerData.Money -= locationCost;
                            playerData.LocationUpgrade++;
                            Sound.PlayAudioFile("buy.wav");
                        }
                    }
                    break;

                case 4: // Back To Menu
                    playerData.GameState = GameState.MainMenu;
                    break;
            }
        }
    }
}
