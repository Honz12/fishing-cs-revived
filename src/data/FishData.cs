namespace fishing_cs_revived.src.Data
{
    /*
        RodLevel:
        - Sladkovodní: 0 až 6
        - Mořské: 2 až 10
        
        Výpočet váhy: Weight + WeightVar * rng (rng = -1.0 až 1.0)
        WeightVar < Weight
    */

    /*
        new TFish()
        {
            Rarity=FishRarity.Common,
            Rarity=FishRarity.Rare,
            Rarity=FishRarity.Epic,
            Rarity=FishRarity.Mythic,
        }
    */

     /*
        Total 22 fish:
            - Common: 8 (Kapr, Okoun, Lín, Amur, Treska, Sardinka, Sleď, Makrela)
            - Rare: 8 (Pstruh, Štika, Candát, Sumec, Jeseter, Losos, Platýs, Pražman)
            - Epic: 3 (Úhoř, Mořský ďas, Mečoun)
            - Mythic: 2 (Tuňák, Čtverzubec)
            - Kraken: 1
    */

    public class FishData
    {
        public static TFish[] fishes = {
            // SLADKOVODNÍ RYBY
            new TFish() {
                Name = "Kapr obecný",
                Rarity = FishRarity.Common,
                Weight = 8.25,
                WeightVar = 6.75,
                RodLevel = 1,
                PricePerKg = 16,
                IsSea = false,
                Image = "kaprObecny.txt"
            },
            new TFish() {
                Name = "Pstruh duhový",
                Rarity = FishRarity.Rare,
                Weight = 1.125,
                WeightVar = 0.875, // 0.25 kg – 2.0 kg
                RodLevel = 1,
                IsSea = false,
                PricePerKg = 195,
                Image = "pstruhDuhovy.txt"
            },
            new TFish() {
                Name = "Štika obecná",
                Rarity = FishRarity.Rare,
                Weight = 4.5,
                WeightVar = 3.5, // 1.0 kg – 8.0 kg
                RodLevel = 2,
                IsSea = false,
                PricePerKg = 66,
                Image = "stikaObecna.txt"
            },
            new TFish() {
                Name = "Candát obecný",
                Rarity = FishRarity.Rare,
                Weight = 3.0,
                WeightVar = 2.0, // 1.0 kg – 5.0 kg
                RodLevel = 2,
                IsSea = false,
                PricePerKg = 140,
                Image = "candatObecny.txt"
            },
            new TFish() {
                Name = "Sumec velký",
                Rarity = FishRarity.Rare,
                Weight = 16.5,
                WeightVar = 13.5, // 3.0 kg – 30.0 kg
                RodLevel = 6,
                IsSea = false,
                PricePerKg = 15,
                Image = "sumecVelky.txt"
            },
            new TFish() {
                Name = "Okoun říční",
                Rarity = FishRarity.Common,
                Weight = 0.45,
                WeightVar = 0.35, // 0.1 kg – 0.8 kg
                RodLevel = 0,
                IsSea = false,
                PricePerKg = 400,
                Image = "okounRicni.txt"
            },
            new TFish() {
                Name = "Lín obecný",
                Rarity = FishRarity.Common,
                Weight = 0.9,
                WeightVar = 0.6, // 0.3 kg – 1.5 kg
                RodLevel = 0,
                IsSea = false,
                PricePerKg = 166,
                Image = "linObecny.txt"
            },
            new TFish() {
                Name = "Jeseter velký",
                Rarity = FishRarity.Rare,
                Weight = 8.5,
                WeightVar = 6.5, // 2.0 kg – 15.0 kg
                RodLevel = 4,
                IsSea = false,
                PricePerKg = 35,
                Image = "jeseterVelky.txt"
            },
            new TFish() {
                Name = "Úhoř říční",
                Rarity = FishRarity.Epic,
                Weight = 0.9,
                WeightVar = 0.6, // 0.3 kg – 1.5 kg
                RodLevel = 1,
                IsSea = false,
                PricePerKg = 722,
                Image = "uhorRicni.txt"
            },
            new TFish() {
                Name = "Amur bílý",
                Rarity = FishRarity.Common,
                Weight = 6.0,
                WeightVar = 4.0, // 2.0 kg – 10.0 kg
                RodLevel = 3,
                IsSea = false,
                PricePerKg = 25,
                Image = "amurBily.txt"
            },

            // MOŘSKÉ RYBY
            new TFish() {
                Name = "Losos obecný",
                Rarity = FishRarity.Rare,
                Weight = 5.0,
                WeightVar = 3.0, // 2.0 kg – 8.0 kg
                RodLevel = 4,
                IsSea = true,
                PricePerKg = 90,
                Image = "lososObecny.txt"
            },
            new TFish() {
                Name = "Treska tmavá",
                Rarity = FishRarity.Common,
                Weight = 3.5,
                WeightVar = 2.5, // 1.0 kg – 6.0 kg
                RodLevel = 3,
                IsSea = true,
                PricePerKg = 80,
                Image = "treskaTmava.txt"
            },
            new TFish() {
                Name = "Tuňák obecný",
                Rarity = FishRarity.Mythic,
                Weight = 135.0,
                WeightVar = 115.0, // 20.0 kg – 250.0 kg
                RodLevel = 8,
                IsSea = true,
                PricePerKg = 7,
                Image = "tunakObecny.txt"
            },
            new TFish() {
                Name = "Sardinka obecná",
                Rarity = FishRarity.Common,
                Weight = 0.06,
                WeightVar = 0.04, // 0.02 kg – 0.1 kg
                RodLevel = 2,
                IsSea = true,
                PricePerKg = 2500,
                Image = "sardinkaObecna.txt"
            },
            new TFish() {
                Name = "Sleď obecný",
                Rarity = FishRarity.Common,
                Weight = 0.25,
                WeightVar = 0.15, // 0.1 kg – 0.4 kg
                RodLevel = 2,
                IsSea = true,
                PricePerKg = 520,
                Image = "sledObecny.txt"
            },
            new TFish() {
                Name = "Makrela obecná",
                Rarity = FishRarity.Common,
                Weight = 0.65,
                WeightVar = 0.35, // 0.3 kg – 1.0 kg
                RodLevel = 2,
                IsSea = true,
                PricePerKg = 292,
                Image = "makrelaObecna.txt"
            },
            new TFish() {
                Name = "Platýs bradavičnatý",
                Rarity = FishRarity.Rare,
                Weight = 1.15,
                WeightVar = 0.85, // 0.3 kg – 2.0 kg
                RodLevel = 3,
                IsSea = true,
                PricePerKg = 304,
                Image = "platysBradavicnaty.txt"
            },
            new TFish() {
                Name = "Pražman zlatý",
                Rarity = FishRarity.Rare,
                Weight = 0.9,
                WeightVar = 0.6, // 0.3 kg – 1.5 kg
                RodLevel = 2,
                IsSea = true,
                PricePerKg = 500,
                Image = "prazmanZlaty.txt"
            },
            new TFish() {
                Name = "Mořský ďas",
                Rarity = FishRarity.Epic,
                Weight = 8.5,
                WeightVar = 6.5, // 2.0 kg – 15.0 kg
                RodLevel = 5,
                IsSea = true,
                PricePerKg = 82,
                Image = "morskyDas.txt"
            },
            new TFish() {
                Name = "Mečoun obecný",
                Rarity = FishRarity.Epic,
                Weight = 115.0,
                WeightVar = 85.0, // 30.0 kg – 200.0 kg
                RodLevel = 7,
                IsSea = true,
                PricePerKg = 7,
                Image = "mecounObecny.txt"
            },
            new TFish() {
                Name = "Čtverzubec fugu",
                Rarity = FishRarity.Mythic,
                Weight = 2.0,
                WeightVar = 1.5, // 0.5 kg – 3.5 kg
                RodLevel = 9,
                IsSea = true,
                PricePerKg = 750,
                Image = "puffer.txt"
            },
            new TFish() {
                Name = "Kraken",
                Rarity = FishRarity.Kraken,
                Weight = 1000.0,
                WeightVar = 100.0, // 900.0 - 1100.0
                RodLevel = 10,
                IsSea = true,
                PricePerKg = 11,
                Image = "kraken.txt"
            }
        };
    }
}
