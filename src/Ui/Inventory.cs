namespace fishing_cs_revived.src.Ui
{
    public class InventoryUi
    {
        public static int Selected = 0;

        /// <summary>
        /// Display the UI.
        /// </summary>
        /// <param name="playerData">Player's data</param>
        public static void DisplayMenu(PlayerData playerData)
        {
            string title = (
    Program.TITLE_COLOR + @"   ___ _    _         _ __   __  ___          " + "\x1b[0m\n" +
    Program.TITLE_COLOR + @"  / __| |_ | |__ _ __| /_/__/_/ | _ ) _____ __" + "\x1b[0m\n" +
    Program.TITLE_COLOR + @" | (__| ' \| / _` / _` | / _| | | _ \/ _ \ \ /" + "\x1b[0m\n" +
    Program.TITLE_COLOR + @"  \___|_||_|_\__,_\__,_|_\__|_| |___/\___/_\_\" + "\x1b[0m\n" 
            );
            Console.WriteLine(title);

            Console.WriteLine("ESC pro zpátky do Hlavního Menu\n");

            if (playerData.Inventory.Count == 0)
            {
                Console.WriteLine("Nemáš nic v Chladícím Boxu.");
                return;
            }

            Console.WriteLine($"Kapacita ({playerData.Inventory.Count} / {Program.GetMaxFishInInventory()})");
            Console.WriteLine();

            for (int i = 0; i < playerData.Inventory.Count; i++)
            {
                if (Selected == i)
                {
                    Console.WriteLine();
                    Program.DisplayImage(playerData.Inventory[i].Image, playerData.Inventory[i].GetFormatedData() + " \x1b[1;37m[S pro Prodání ryby]", "\x1b[96m");
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine(playerData.Inventory[i].GetInfoCompact());
                }
            }
        }

        /// <summary>
        /// Called when the user presses <code>ConsoleKey.DownArrow</code>
        /// </summary>
        /// <param name="playerData">Player's data</param>
        public static void UiButtonMenuDown(PlayerData playerData)
        {
            if (playerData.Inventory.Count == 0) return;
            Selected++;
            Selected += playerData.Inventory.Count;
            Selected %= playerData.Inventory.Count;
        }

        /// <summary>
        /// Called when the user presses <code>ConsoleKey.UpArrow</code>
        /// </summary>
        /// <param name="playerData">Player's data</param>
        public static void UiButtonMenuUp(PlayerData playerData)
        {
            if (playerData.Inventory.Count == 0) return;
            Selected--;
            Selected += playerData.Inventory.Count;
            Selected %= playerData.Inventory.Count;
        }

        /// <summary>
        /// Sells the currently selected item.
        /// </summary>
        /// <param name="playerData">Player's data</param>
        public static void SellOption(PlayerData playerData)
        {
            if (playerData.Inventory.Count == 0) return;

            Console.Clear();

            Selected %= playerData.Inventory.Count;

            playerData.Money += (uint) (playerData.Inventory[Selected].PricePerKg * playerData.Inventory[Selected].Weight * Program.GetMoneyMultiplier());

            playerData.Inventory.RemoveAt(Selected);
            Selected = Math.Max(0, Selected - 1);
            Sound.PlayAudioFile("sell.wav");
        }
    }
}
