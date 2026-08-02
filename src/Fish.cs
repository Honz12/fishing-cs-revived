using fishing_cs_revived.src.Data;

namespace fishing_cs_revived.src
{
    public class FishSaveData
    {
        public int Id;
        public double Weight;
    }

    public class Fish
    {
        public string Name;
        public double Weight;
        public int RodLevel;
        public bool IsSea;
        public Image Image;
        public FishRarity Rarity;
        public double PricePerKg;
        public double AverageWeight;
        public int Id;

        public Fish(int id) // Constructor
        {
            TFish template = FishData.fishes[id];

            Name = template.Name;
            Weight = Math.Round((template.Weight + template.WeightVar * (Program.Rng.NextDouble() * 2.0 - 1.0)) * 100.0) / 100.0;
            RodLevel = template.RodLevel;
            IsSea = template.IsSea;
            Image = new Image("fish", template.Image);
            Rarity = template.Rarity;
            PricePerKg = template.PricePerKg;
            AverageWeight = template.Weight;
            Id = id;
        }

        public Fish() // Slop Constructor
        {
            Name = "";
            Weight = 0.0;
            RodLevel = 0;
            IsSea = false;
            Image = new Image("fish", "uhorRicniEletricky.img");
            Rarity = FishRarity.Common;
            PricePerKg = 0.0;
            Id = -1;
        }

        /// <summary>
        /// Returns the full formated data of the fish,
        /// meant to be used with the <code>Program.DisplayImage</code> function
        /// </summary>
        /// <returns>The formated string.</returns>
        public string GetFormatedData()
        {
            string isFromSea = IsSea ? "Mořská" : "Sladkovodní";

            string s = $"{Name}\n- Váha: {Weight} Kg (Průměr {AverageWeight} Kg)\n- {isFromSea}\n- Vzácnost: {Program.GetTransRarity(Rarity)}\n- Požadovaná úroveň prutu: {RodLevel+1}\n- Prodává se za: {(uint) (PricePerKg * Weight * Program.GetMoneyMultiplier())}";

            if (Program.GetMoneyMultiplier() != 1.0)
            {
                s += $" ({(uint) (PricePerKg * Weight)} x{Program.GetMoneyMultiplier()})";
            }

            return s;
        }

        /// <summary>
        /// Returns the compact formated string of the fish,
        /// meant to be used in <code>Inventory.cs</code>.
        /// </summary>
        /// <returns>The formated string.</returns>
        public string GetInfoCompact()
        {
            return $"{Name, -20} | Váha: {Weight, -6} Kg | {Program.GetTransRarity(Rarity) + Program.RepeatString(" ", 12 - Program.GetTransRarityNoColor(Rarity).Length)} | Cena: {(uint) (PricePerKg * Weight)}";
        }

        /// <summary>
        /// Gets the save data of the fish.
        /// </summary>
        /// <returns></returns>
        public FishSaveData GetSaveData()
        {
            return new()
            {
                Id = Id,
                Weight = Weight
            };
        }
    }
}