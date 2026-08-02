using fishing_cs_revived.src.Data;

namespace fishing_cs_revived.src
{
    public class Advancement
    {
        public required string Name;
        public required string Description;
        public required string IconName;
        public required string Id;
    }

    public static class AdvancementProcessor
    {
        /// <summary>
        /// Checks if the player has unlocked any new advancements based on their current data.
        /// If not returns null, otherwise returns the Advancement object that was unlocked.
        /// </summary>
        /// <param name="playerData"></param>
        /// <returns></returns>
        public static int AdvancementCount = 6;
        public static Advancement? CheckForNewAdvancements(PlayerData playerData)
        {
            bool hasKraken = false;

            foreach (Fish fish in Program.data.Inventory)
            {
                if (fish.Rarity == FishRarity.Special)
                {
                    hasKraken = true;
                    break;
                }
            }

            if (playerData.Inventory.Count > 0 && !playerData.UnlockedAdvancementIds.Contains("prvniUlovek"))
            {
                playerData.UnlockedAdvancementIds.Add("prvniUlovek");
                return new Advancement()
                {
                    Name = "1. Úlovek!",
                    Description = "Chyť svou první rybu.",
                    IconName = "prvniUlovek.img",
                    Id = "prvniUlovek"
                };
            }

            else if (Program.data.RodLevel == 1 && !playerData.UnlockedAdvancementIds.Contains("poradnyPrut"))
            {
                playerData.UnlockedAdvancementIds.Add("poradnyPrut");
                return new Advancement()
                {
                    Name = "Konečně pořádný prut!",
                    Description = "Upgraduj poprvé svůj prut.",
                    IconName = "poradnyPrut.img",
                    Id = "poradnyPrut"
                };
            }
            
            else if (Program.data.HouseLevel == 1 && !playerData.UnlockedAdvancementIds.Contains("domaNejlip"))
            {
                playerData.UnlockedAdvancementIds.Add("domaNejlip");
                return new Advancement()
                {
                    Name = "Všude dobře, doma nejlíp!",
                    Description = "Kup si svůj první dům.",
                    IconName = "domaNejlip.img",
                    Id = "domaNejlip"
                };
            }
            
            else if (hasKraken && !playerData.UnlockedAdvancementIds.Contains("chytKrakena"))
            {
                playerData.UnlockedAdvancementIds.Add("chytKrakena");
                return new Advancement()
                {
                    Name = "Co to je?!",
                    Description = "Chyť Krakena.",
                    IconName = "chytKrakena.img",
                    Id = "chytKrakena"
                };
            }
            
            else if (Program.data.HouseLevel >= 4 && !playerData.UnlockedAdvancementIds.Contains("tatuvTelefonat"))
            {
                playerData.UnlockedAdvancementIds.Add("tatuvTelefonat");
                return new Advancement()
                {
                    Name = "Tátův telefonát.",
                    Description = "Táta ti zavolal.",
                    IconName = "tatuvTelefonat.img",
                    Id = "tatuvTelefonat"
                };
            }

            else if (Program.data.HouseLevel >= 4 && Program.data.RodLevel == 10 && Program.data.InventorySize >= 4 && !playerData.UnlockedAdvancementIds.Contains("max"))
            {
                playerData.UnlockedAdvancementIds.Add("max");
                return new Advancement()
                {
                    Name = "Max!",
                    Description = "Koupil jsi všechny upgrady.",
                    IconName = "max.img",
                    Id = "max"
                };
            }

            return null; 
        }
    }
}
