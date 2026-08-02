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
    }
}