public enum FishRarity
{
    Common,
    Rare,
    Epic,
    Mythic,
    Kraken,
}

public class TFish // The "TemplateFish".
{
    public required string Name;
    public required double Weight;
    public required double WeightVar;
    public required int RodLevel;
    public required bool IsSea;
    public required string Image;
    public required FishRarity Rarity;
    public required double PricePerKg;
}
