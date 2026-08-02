namespace fishing_cs_revived.src.Data
{
    public static class TFishFinder
    {
        /// <summary>
        /// Find a random available fish.
        /// </summary>
        /// <param name="rodLevel">Player's Fishing Rod level.</param>
        /// <returns>The fish Id</returns>
        public static int FindRandomFish(int rodLevel, FishLocation location)
        {
            List<int> possible = new List<int>();

            for (int i = 0; i < FishData.fishes.Length; i++)
            {
                TFish fish = FishData.fishes[i];

                if (rodLevel >= fish.RodLevel && fish.AvaiableLocations.Contains(location))
                {
                    for (int j = 0; j < fish.Chance; j++)
                    {
                        possible.Add(i);
                    }
                }
            }

            if (possible.Count == 0)
            {
                return -1;
            }

            return possible[Program.Rng.Next(0, possible.Count)];
        }
    }
}