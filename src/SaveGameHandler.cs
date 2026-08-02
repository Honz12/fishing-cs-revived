using System.Text.Json;

namespace fishing_cs_revived.src
{
    public class SaveFileData
    {
        public uint Money = 0;
        public ushort RodLevel = 0;
        public byte InventorySize = 0;
        public byte HouseLevel = 0;

        public List<FishSaveData> Inventory = [];

        public List<Advancement> Advancements = [];
        public List<string> UnlockedAdvancementIds = [];

        public List<int> UnlockedFishIds = [];
    }

    public class SaveGameHandler
    {
        static readonly JsonSerializerOptions options = new()
        {
            WriteIndented = true,
            IncludeFields = true
        };

        /// <summary>
        /// Saves the game to ~/kjr/save.json
        /// </summary>
        /// <param name="playerData">The player data.</param>
        /// <returns>The save success.</returns>
        public static bool SaveGame(PlayerData playerData)
        {
            SaveFileData saveFile = new()
            {
                Money = playerData.Money,
                RodLevel = playerData.RodLevel,
                InventorySize = playerData.InventorySize,
                HouseLevel = playerData.HouseLevel,

                Advancements = playerData.Advancements,
                UnlockedAdvancementIds = playerData.UnlockedAdvancementIds,

                UnlockedFishIds = playerData.UnlockedFishIds
            };

            foreach (Fish fish in playerData.Inventory)
            {
                saveFile.Inventory.Add(fish.GetSaveData());
            }

            try
            {
                string userDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string saveDir = Path.Combine(userDir, "kjr");
                Directory.CreateDirectory(saveDir);
                string jsonString = JsonSerializer.Serialize(saveFile, options);
                File.WriteAllText(Path.Combine(saveDir, "save.json"), jsonString);
                return true;
            }
            catch
            {
                Console.WriteLine("Error writing save file");
                Console.ReadKey();
            }
            return false;
        }

        /// <summary>
        /// Loads the game from ~/kjr/save.json
        /// </summary>
        /// <param name="playerData">The player data.</param>
        /// <returns>The load success.</returns>
        public static bool LoadGame(PlayerData playerData, bool pauseIfUnsuccessful = true)
        {
            try
            {
                string userDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string saveDir = Path.Combine(userDir, "kjr");
                string jsonString = File.ReadAllText(Path.Combine(saveDir, "save.json"));
                Console.WriteLine(jsonString);
                SaveFileData saveFile = JsonSerializer.Deserialize<SaveFileData>(jsonString, options)!;

                playerData.Money = saveFile.Money;
                playerData.RodLevel = saveFile.RodLevel;
                playerData.InventorySize = saveFile.InventorySize;
                playerData.HouseLevel = saveFile.HouseLevel;

                playerData.Inventory = [];

                playerData.Advancements = saveFile.Advancements;
                playerData.UnlockedAdvancementIds = saveFile.UnlockedAdvancementIds;

                playerData.UnlockedFishIds = saveFile.UnlockedFishIds;

                foreach (FishSaveData fishSaveData in saveFile.Inventory)
                {
                    Fish f = new(fishSaveData.Id)
                    {
                        Weight = fishSaveData.Weight
                    };
                    playerData.Inventory.Add(f);
                }

                return true;
            }
            catch
            {
                if (pauseIfUnsuccessful)
                {
                    Console.WriteLine("Error loading save file");
                    Console.ReadKey();
                }
            }
            return false;
        }
    }
}