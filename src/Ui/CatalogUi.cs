using fishing_cs_revived.src.Data;

namespace fishing_cs_revived.src.Ui
{
    public class CatalogUi
    {
        static readonly List<int> unlockedFishIds = [];

        public static void ShowCatalog()
        {
            for (int i = 0; i < FishData.fishes.Length; i++)
            {
                TFish tFish = FishData.fishes[i];

                if (unlockedFishIds.Contains(i))
                    Console.WriteLine($"Name: {tFish.Name,-20}, Rarity: {tFish.Rarity}");
                else
                    Console.WriteLine($"Name: ???, Rarity: ???");
            }
        }

        public static void UnlockFish(int fishId)
        {
            if (!unlockedFishIds.Contains(fishId))
                unlockedFishIds.Add(fishId);
        }
    }
}