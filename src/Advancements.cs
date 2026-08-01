class Advancement
{
    public required string Name;
    public required string Description;
    public required string IconName;
    public required string Id;
}



static class AdvancementProcessor
{
    static List<string> unlockedAdvancementIds = new();

    /// <summary>
    /// Checks if the player has unlocked any new advancements based on their current data.
    /// If not returns null, otherwise returns the Advancement object that was unlocked.
    /// </summary>
    /// <param name="playerData"></param>
    /// <returns></returns>
    public static int AdvancementCount = 0;
    public static Advancement? CheckForNewAdvancements(PlayerData playerData)
    {
        if (playerData.Inventory.Count > 0 && !unlockedAdvancementIds.Contains("prvniUlovek"))
        {
            unlockedAdvancementIds.Add("prvniUlovek");
            return new Advancement()
            {
                Name = "1. Úlovek!",
                Description = "Chyť svou první rybu.",
                IconName = "prvniUlovek.txt",
                Id = "prvniUlovek"
            };
        }

        else if (Program.data.RodLevel == 1 && !unlockedAdvancementIds.Contains("poradnyPrut"))
        {
            unlockedAdvancementIds.Add("poradnyPrut");
            return new Advancement()
            {
                Name = "Konečně pořádný prut!",
                Description = "Upgraduj poprvé svůj prut.",
                IconName = "poradnyPrut.txt",
                Id = "poradnyPrut"
            };
        }
        
        else if (Program.data.HouseLevel == 1 && !unlockedAdvancementIds.Contains("domaNejlip"))
        {
            unlockedAdvancementIds.Add("domaNejlip");
            return new Advancement()
            {
                Name = "Všude dobře, doma nejlíp!",
                Description = "Kup si svůj první dům.",
                IconName = "domaNejlip.txt",
                Id = "domaNejlip"
            };
        }
        
        else if (!unlockedAdvancementIds.Contains("chytKrakena"))
        {
            bool hasKraken = false;

            foreach (Fish fish in Program.data.Inventory)
            {
                if (fish.Rarity == FishRarity.Kraken)
                {
                    hasKraken = true;
                    break;
                }
            }

            if (hasKraken)
            {
                unlockedAdvancementIds.Add("chytKrakena");
                return new Advancement()
                {
                    Name = "Co to je?!",
                    Description = "Chyť Krakena.",
                    IconName = "chytKrakena.txt",
                    Id = "chytKrakena"
                };
            }
        }
        
        else if (Program.data.HouseLevel >= 4 && !unlockedAdvancementIds.Contains("tatuvTelefonat"))
        {
            unlockedAdvancementIds.Add("tatuvTelefonat");
            return new Advancement()
            {
                Name = "Tátův telefonát.",
                Description = "Táta ti zavolal.",
                IconName = "tatuvTelefonat.txt",
                Id = "tatuvTelefonat.txt"
            };
        }
        return null; 
    }
}