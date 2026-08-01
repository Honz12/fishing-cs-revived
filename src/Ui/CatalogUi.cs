using fishing_cs_revived.src.Data;

namespace fishing_cs_revived.src.Ui
{
    public class CatalogUi
    {
        static readonly List<int> unlockedFishIds = [];

        public static void ShowCatalog()
        {
            string title = Program.TITLE_COLOR + @"  _  __     _        _             ___      _    
 | |/ /__ _| |_ __ _| |___  __ _  | _ \_  _| |__ 
 | ' </ _` |  _/ _` | / _ \/ _` | |   / || | '_ \
 |_|\_\__,_|\__\__,_|_\___/\__, | |_|_\\_, |_.__/
                           |___/       |__/      " + "\n\x1b[0m";
            
            Console.WriteLine(title);

            string line = "";
            string unknown = "???";

            for (int i = 0; i < FishData.fishes.Length; i++)
            {
                TFish tFish = FishData.fishes[i];

                if (unlockedFishIds.Contains(i))
                    line += $"Name: {tFish.Name,-20} Rarity: {Program.GetTransRarity(tFish.Rarity) + Program.RepeatString(" ", 12 - Program.GetTransRarityNoColor(tFish.Rarity).Length)} | ";
                else
                    line += $"\x1b[0;90mName: {unknown,-20} Rarity: {unknown,-12}\x1b[0m | ";
                
                if (i % 2 == 1)
                {
                    Console.WriteLine("| " + line);
                    line = "";
                }
            }
            if (line.Length != 0)
                Console.WriteLine("| " + line);
        }

        public static void UnlockFish(int fishId)
        {
            if (!unlockedFishIds.Contains(fishId))
                unlockedFishIds.Add(fishId);
        }
    }
}