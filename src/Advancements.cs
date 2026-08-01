class Advancement
{
    public required string Name;
    public required string Description;
    public required string IconName;
}

static class AdvancementProcessor
{
    /// <summary>
    /// Checks if the player has unlocked any new advancements based on their current data.
    /// If not returns null, otherwise returns the Advancement object that was unlocked.
    /// </summary>
    /// <param name="playerData"></param>
    /// <returns></returns>
    public static Advancement? CheckForNewAdvancements(PlayerData playerData)
    {
        return null;
    }
}
