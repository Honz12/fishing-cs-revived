class Advancement
{
    public required string Name;
    public required string Description;
    public required string IconName;
    public required bool IsUnlocked;
}



static class AdvancementProcessor
{
    /// <summary>
    /// Checks if the player has unlocked any new advancements based on their current data.
    /// If not returns null, otherwise returns the Advancement object that was unlocked.
    /// </summary>
    /// <param name="playerData"></param>
    /// <returns></returns>
    public static int AdvancementCount = 0;
    public static Advancement? CheckForNewAdvancements(PlayerData playerData)
    {
        return null;
    }
}

class AdvancementData
{
    public static Advancement[] advancements =
    {
        new Advancement()
        {
            Name = "1. Úlovek!",
            Description = "Chyť svou první rybu.",
            IconName = "prvniUlovek.txt",
            IsUnlocked = false
        },
        new Advancement()
        {
            Name = "Konečně pořádný prut!",
            Description = "Upgraduj poprvé svůj prut.",
            IconName = "poradnyPrut.txt",
            IsUnlocked = false
        },
        new Advancement()
        {
            Name = "Všude dobře, doma nejlíp!",
            Description = "Kup si svůj první dům.",
            IconName = "domaNejlip.txt",
            IsUnlocked = false
        },
        new Advancement()
        {
            Name = "Co to je?!",
            Description = "Chyť Krakena.",
            IconName = "chytKrakena.txt",
            IsUnlocked = false
        },
        new Advancement()
        {
            Name = "Tátův telefonát.",
            Description = "Táta ti zavolal.",
            IconName = "tatuvTelefonat.txt",
            IsUnlocked = false
        }
    };
}
