using fishing_cs_revived.src.Data;

namespace fishing_cs_revived.src.Ui
{
    public class CatalogUi
    {
        static readonly List<int> unlockedFishIds = [];

        private static void DisplayTableRarity(FishRarity rarity)
        {
            Console.WriteLine(Program.TITLE_COLOR + "===== " + Program.GetTransRarity(rarity) + Program.TITLE_COLOR + " =====\x1b[0m");

            string line = "";
            string unknown = "???";

            int j = 0;

            for (int i = 0; i < FishData.fishes.Length; i++)
            {
                TFish tFish = FishData.fishes[i];
                if (tFish.Rarity != rarity)
                    continue;

                if (unlockedFishIds.Contains(i))
                    line += $"{tFish.Name,-20} | ";
                else
                    line += $"\x1b[0;90m{unknown,-20}\x1b[0m | ";
                
                if (j % 3 == 2)
                {
                    Console.WriteLine("| " + line);
                    line = "";
                }
                j++;
            }
            if (line.Length != 0)
                Console.WriteLine("| " + line);
        }

        private static void DisplayTableRarityOther()
        {
            Console.WriteLine(Program.TITLE_COLOR + "===== Jiné =====\x1b[0m");

            string line = "";
            string unknown = "???";

            int j = 0;

            for (int i = 0; i < FishData.fishes.Length; i++)
            {
                TFish tFish = FishData.fishes[i];
                if (tFish.Rarity != FishRarity.Kraken)
                    continue;

                if (unlockedFishIds.Contains(i))
                    line += $"{tFish.Name,-20} | ";
                else
                    line += $"\x1b[0;90m{unknown,-20}\x1b[0m | ";
                
                if (j % 3 == 2)
                {
                    Console.WriteLine("| " + line);
                    line = "";
                }
                j++;
            }
            if (line.Length != 0)
                Console.WriteLine("| " + line);
        }

        public static void ShowCatalog()
        {
            string title = Program.TITLE_COLOR + @"   _  __     _        _             ___      _    
  | |/ /__ _| |_ __ _| |___  __ _  | _ \_  _| |__ 
  | ' </ _` |  _/ _` | / _ \/ _` | |   / || | '_ \
  |_|\_\__,_|\__\__,_|_\___/\__, | |_|_\\_, |_.__/
                             |___/       |__/      " + "\n\x1b[0m";
            
            Console.WriteLine(title);

            DisplayTableRarity(FishRarity.Common);
            DisplayTableRarity(FishRarity.Rare);
            DisplayTableRarity(FishRarity.Epic);
            DisplayTableRarity(FishRarity.Mythic);
            DisplayTableRarityOther();
        }

        public static void UnlockFish(int fishId)
        {
            if (!unlockedFishIds.Contains(fishId))
                unlockedFishIds.Add(fishId);
        }

        public static void UnUnlockFish(int fishId)
        {
            if (unlockedFishIds.Contains(fishId))
                unlockedFishIds.Remove(fishId);
        }
    }
}