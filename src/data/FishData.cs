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

        kazda ryba musi mit 
                AvaiableLocations = [ ..., ..., ... ]
        muze jich v [] byt kolik chces :D
        priklady jsou Kapr a Losos
    */

    public class FishData
    {
        public static TFish[] fishes = {
            // SLADKOVODNÍ RYBY
            new TFish() {
                Name = "Kapr obecný",
                Rarity = FishRarity.Common,
                Chance = 10,
                Weight = 8.25,
                WeightVar = 6.75,
                RodLevel = 1,
                PricePerKg = 16,
                IsSea = false,
                Image = "kaprObecny.img",
                AvaiableLocations = [FishLocation.Lake]
            },
            new TFish() {
                Name = "Pstruh duhový",
                Rarity = FishRarity.Rare,
                Chance = 5,
                Weight = 1.125,
                WeightVar = 0.875, // 0.25 kg – 2.0 kg
                RodLevel = 1,
                IsSea = false,
                PricePerKg = 195,
                Image = "pstruhDuhovy.img",
                AvaiableLocations = [FishLocation.Lake]
            },
            new TFish() {
                Name = "Štika obecná",
                Rarity = FishRarity.Rare,
                Chance = 5,
                Weight = 4.5,
                WeightVar = 3.5, // 1.0 kg – 8.0 kg
                RodLevel = 2,
                IsSea = false,
                PricePerKg = 66,
                Image = "stikaObecna.img",
                AvaiableLocations = [FishLocation.Lake]
            },
            new TFish() {
                Name = "Candát obecný",
                Rarity = FishRarity.Rare,
                Chance = 5,
                Weight = 3.0,
                WeightVar = 2.0, // 1.0 kg – 5.0 kg
                RodLevel = 2,
                IsSea = false,
                PricePerKg = 140,
                Image = "candatObecny.img",
                AvaiableLocations = [FishLocation.Lake]
            },
            new TFish() {
                Name = "Sumec velký",
                Rarity = FishRarity.Rare,
                Chance = 5,
                Weight = 16.5,
                WeightVar = 13.5, // 3.0 kg – 30.0 kg
                RodLevel = 6,
                IsSea = false,
                PricePerKg = 15,
                Image = "sumecVelky.img",
                AvaiableLocations = [FishLocation.Lake]
            },
            new TFish() {
                Name = "Okoun říční",
                Rarity = FishRarity.Common,
                Chance = 10,
                Weight = 0.45,
                WeightVar = 0.35, // 0.1 kg – 0.8 kg
                RodLevel = 0,
                IsSea = false,
                PricePerKg = 400,
                Image = "okounRicni.img",
                AvaiableLocations = [FishLocation.Lake]
            },
            new TFish() {
                Name = "Lín obecný",
                Rarity = FishRarity.Common,
                Chance = 10,
                Weight = 0.9,
                WeightVar = 0.6, // 0.3 kg – 1.5 kg
                RodLevel = 0,
                IsSea = false,
                PricePerKg = 166,
                Image = "linObecny.img",
                AvaiableLocations = [FishLocation.Lake]
            },
            new TFish() {
                Name = "Jeseter velký",
                Rarity = FishRarity.Rare,
                Chance = 5,
                Weight = 8.5,
                WeightVar = 6.5, // 2.0 kg – 15.0 kg
                RodLevel = 4,
                IsSea = false,
                PricePerKg = 35,
                Image = "jeseterVelky.img",
                AvaiableLocations = [FishLocation.Lake]
            },
            new TFish() {
                Name = "Úhoř říční",
                Rarity = FishRarity.Epic,
                Chance = 2,
                Weight = 0.9,
                WeightVar = 0.6, // 0.3 kg – 1.5 kg
                RodLevel = 1,
                IsSea = false,
                PricePerKg = 722,
                Image = "uhorRicni.img",
                AvaiableLocations = [FishLocation.Lake]
            },
            new TFish() {
                Name = "Amur bílý",
                Rarity = FishRarity.Common,
                Chance = 10,
                Weight = 6.0,
                WeightVar = 4.0, // 2.0 kg – 10.0 kg
                RodLevel = 3,
                IsSea = false,
                PricePerKg = 25,
                Image = "amurBily.img",
                AvaiableLocations = [FishLocation.Lake]
            },
            new TFish() {
                Name = "Plotice obecná",
                Rarity = FishRarity.Common,
                Chance = 10,
                Weight = 0.3,
                WeightVar = 0.2, // 0.1 kg – 0.5 kg
                RodLevel = 0,
                IsSea = false,
                PricePerKg = 100,
                Image = "ploticeObecna.img",
                AvaiableLocations = [FishLocation.Lake]
            },
            new TFish() {
                Name = "Ouklej obecná",
                Rarity = FishRarity.Common,
                Chance = 10,
                Weight = 0.05,
                WeightVar = 0.03, // 0.02 kg – 0.08 kg
                RodLevel = 0,
                IsSea = false,
                PricePerKg = 450,
                Image = "ouklejObecna.img",
                AvaiableLocations = [FishLocation.Lake]
            },
            new TFish() {
                Name = "Cejn velký",
                Rarity = FishRarity.Common,
                Chance = 10,
                Weight = 2.0,
                WeightVar = 1.0, // 1.0 kg – 3.0 kg
                RodLevel = 1,
                IsSea = false,
                PricePerKg = 30,
                Image = "cejnVelky.img",
                AvaiableLocations = [FishLocation.Lake]
            },
            new TFish() {
                Name = "Karas stříbřitý",
                Rarity = FishRarity.Common,
                Chance = 10,
                Weight = 0.4,
                WeightVar = 0.3, // 0.1 kg – 0.7 kg
                RodLevel = 0,
                IsSea = false,
                PricePerKg = 65,
                Image = "karasStribrity.img",
                AvaiableLocations = [FishLocation.Lake]
            },
            new TFish() {
                Name = "Bolen dravý",
                Rarity = FishRarity.Rare,
                Chance = 5,
                Weight = 3.0,
                WeightVar = 2.0, // 1.0 kg – 5.0 kg
                RodLevel = 3,
                IsSea = false,
                PricePerKg = 45,
                Image = "bolenDravey.img",
                AvaiableLocations = [FishLocation.Lake]
            },
            new TFish() {
                Name = "Parma obecná",
                Rarity = FishRarity.Rare,
                Chance = 5,
                Weight = 2.5,
                WeightVar = 1.5, // 1.0 kg – 4.0 kg
                RodLevel = 2,
                IsSea = false,
                PricePerKg = 50,
                Image = "parmaObecna.img",
                AvaiableLocations = [FishLocation.Lake]
            },
            new TFish() {
                Name = "Pstruh potoční",
                Rarity = FishRarity.Rare,
                Chance = 5,
                Weight = 0.8,
                WeightVar = 0.5, // 0.3 kg – 1.3 kg
                RodLevel = 1,
                IsSea = false,
                PricePerKg = 250,
                Image = "pstruhPotocni.img",
                AvaiableLocations = [FishLocation.Lake]
            },
            new TFish() {
                Name = "Siven americký",
                Rarity = FishRarity.Epic,
                Chance = 2,
                Weight = 1.5,
                WeightVar = 1.0, // 0.5 kg – 2.5 kg
                RodLevel = 2,
                IsSea = false,
                PricePerKg = 300,
                Image = "sivenAmericky.img",
                AvaiableLocations = [FishLocation.Lake]
            },
            new TFish() {
                Name = "Vyza velká",
                Rarity = FishRarity.Mythic,
                Chance = 1,
                Weight = 500.0,
                WeightVar = 400.0, // 100.0 kg – 900.0 kg
                RodLevel = 6,
                IsSea = false,
                PricePerKg = 30,
                Image = "vyzaVelka.img",
                AvaiableLocations = [FishLocation.Lake]
            },
            new TFish() {
                Name = "Arapaima velká",
                Rarity = FishRarity.Special,
                Chance = 1,
                Weight = 100.0,
                WeightVar = 80.0, // 20.0 kg – 180.0 kg
                RodLevel = 6,
                IsSea = false,
                PricePerKg = 40,
                Image = "arapaimaVelka.img",
                AvaiableLocations = [FishLocation.Lake]
            },

            // MOŘSKÉ RYBY
            new TFish() {
                Name = "Losos obecný",
                Rarity = FishRarity.Rare,
                Chance = 5,
                Weight = 5.0,
                WeightVar = 3.0, // 2.0 kg – 8.0 kg
                RodLevel = 4,
                IsSea = true,
                PricePerKg = 90,
                Image = "lososObecny.img",
                AvaiableLocations = [FishLocation.Lake, FishLocation.Sea]
            },
            new TFish() {
                Name = "Treska tmavá",
                Rarity = FishRarity.Common,
                Chance = 10,
                Weight = 3.5,
                WeightVar = 2.5, // 1.0 kg – 6.0 kg
                RodLevel = 3,
                IsSea = true,
                PricePerKg = 80,
                Image = "treskaTmava.img",
                AvaiableLocations = [FishLocation.Sea]
            },
            new TFish() {
                Name = "Tuňák obecný",
                Rarity = FishRarity.Mythic,
                Chance = 1,
                Weight = 135.0,
                WeightVar = 115.0, // 20.0 kg – 250.0 kg
                RodLevel = 8,
                IsSea = true,
                PricePerKg = 7,
                Image = "tunakObecny.img",
                AvaiableLocations = [FishLocation.Sea]
            },
            new TFish() {
                Name = "Sardinka obecná",
                Rarity = FishRarity.Common,
                Chance = 10,
                Weight = 0.06,
                WeightVar = 0.04, // 0.02 kg – 0.1 kg
                RodLevel = 2,
                IsSea = true,
                PricePerKg = 2500,
                Image = "sardinkaObecna.img",
                AvaiableLocations = [FishLocation.Sea]
            },
            new TFish() {
                Name = "Sleď obecný",
                Rarity = FishRarity.Common,
                Chance = 10,
                Weight = 0.25,
                WeightVar = 0.15, // 0.1 kg – 0.4 kg
                RodLevel = 2,
                IsSea = true,
                PricePerKg = 520,
                Image = "sledObecny.img",
                AvaiableLocations = [FishLocation.Sea]
            },
            new TFish() {
                Name = "Makrela obecná",
                Rarity = FishRarity.Common,
                Chance = 10,
                Weight = 0.65,
                WeightVar = 0.35, // 0.3 kg – 1.0 kg
                RodLevel = 2,
                IsSea = true,
                PricePerKg = 292,
                Image = "makrelaObecna.img",
                AvaiableLocations = [FishLocation.Sea]
            },
            new TFish() {
                Name = "Platýs bradavičnatý",
                Rarity = FishRarity.Rare,
                Chance = 5,
                Weight = 1.15,
                WeightVar = 0.85, // 0.3 kg – 2.0 kg
                RodLevel = 3,
                IsSea = true,
                PricePerKg = 304,
                Image = "platysBradavicnaty.img",
                AvaiableLocations = [FishLocation.Sea]
            },
            new TFish() {
                Name = "Pražman zlatý",
                Rarity = FishRarity.Rare,
                Chance = 5,
                Weight = 0.9,
                WeightVar = 0.6, // 0.3 kg – 1.5 kg
                RodLevel = 2,
                IsSea = true,
                PricePerKg = 500,
                Image = "prazmanZlaty.img",
                AvaiableLocations = [FishLocation.Sea]
            },
            new TFish() {
                Name = "Mořský ďas",
                Rarity = FishRarity.Epic,
                Chance = 2,
                Weight = 8.5,
                WeightVar = 6.5, // 2.0 kg – 15.0 kg
                RodLevel = 5,
                IsSea = true,
                PricePerKg = 82,
                Image = "morskyDas.img",
                AvaiableLocations = [FishLocation.DeepSea]
            },
            new TFish() {
                Name = "Mečoun obecný",
                Rarity = FishRarity.Epic,
                Chance = 2,
                Weight = 115.0,
                WeightVar = 85.0, // 30.0 kg – 200.0 kg
                RodLevel = 7,
                IsSea = true,
                PricePerKg = 7,
                Image = "mecounObecny.img",
                AvaiableLocations = [FishLocation.Sea]
            },
            new TFish() {
                Name = "Čtverzubec fugu",
                Rarity = FishRarity.Mythic,
                Chance = 1,
                Weight = 2.0,
                WeightVar = 1.5, // 0.5 kg – 3.5 kg
                RodLevel = 9,
                IsSea = true,
                PricePerKg = 750,
                Image = "puffer.img",
                AvaiableLocations = [FishLocation.Sea, FishLocation.DeepSea]
            },
            new TFish() {
                Name = "Šprot obecný",
                Rarity = FishRarity.Common,
                Chance = 10,
                Weight = 0.04,
                WeightVar = 0.02, // 0.02 kg – 0.06 kg
                RodLevel = 2,
                IsSea = true,
                PricePerKg = 1200,
                Image = "sprotObecny.img",
                AvaiableLocations = [FishLocation.Sea]
            },
            new TFish() {
                Name = "Kranas obecný",
                Rarity = FishRarity.Common,
                Chance = 10,
                Weight = 1.0,
                WeightVar = 0.6, // 0.4 kg – 1.6 kg
                RodLevel = 2,
                IsSea = true,
                PricePerKg = 180,
                Image = "kranasObecny.img",
                AvaiableLocations = [FishLocation.Sea]
            },
            new TFish() {
                Name = "Hejk obecný",
                Rarity = FishRarity.Rare,
                Chance = 5,
                Weight = 1.5,
                WeightVar = 1.0, // 0.5 kg – 2.5 kg
                RodLevel = 3,
                IsSea = true,
                PricePerKg = 150,
                Image = "hejkObecny.img",
                AvaiableLocations = [FishLocation.Sea]
            },
            new TFish() {
                Name = "Treska jednoskvrnná",
                Rarity = FishRarity.Rare,
                Chance = 5,
                Weight = 2.0,
                WeightVar = 1.5, // 0.5 kg – 3.5 kg
                RodLevel = 3,
                IsSea = true,
                PricePerKg = 70,
                Image = "treskaJednoskvrnna.img",
                AvaiableLocations = [FishLocation.Sea]
            },
            new TFish() {
                Name = "Okouník mořský",
                Rarity = FishRarity.Rare,
                Chance = 5,
                Weight = 1.2,
                WeightVar = 0.8, // 0.4 kg – 2.0 kg
                RodLevel = 3,
                IsSea = true,
                PricePerKg = 200,
                Image = "okounikMorsky.img",
                AvaiableLocations = [FishLocation.Sea]
            },
            new TFish() {
                Name = "Úhoř mořský",
                Rarity = FishRarity.Epic,
                Chance = 2,
                Weight = 8.0,
                WeightVar = 6.0, // 2.0 kg – 14.0 kg
                RodLevel = 5,
                IsSea = true,
                PricePerKg = 60,
                Image = "uhorMorsky.img",
                AvaiableLocations = [FishLocation.Sea]
            },
            new TFish() {
                Name = "Barakuda obecná",
                Rarity = FishRarity.Epic,
                Chance = 2,
                Weight = 10.0,
                WeightVar = 8.0, // 2.0 kg – 18.0 kg
                RodLevel = 6,
                IsSea = true,
                PricePerKg = 40,
                Image = "barakudaObecna.img",
                AvaiableLocations = [FishLocation.Sea]
            },
            new TFish() {
                Name = "Rejnok obecný",
                Rarity = FishRarity.Epic,
                Chance = 2,
                Weight = 20.0,
                WeightVar = 15.0, // 5.0 kg – 35.0 kg
                RodLevel = 5,
                IsSea = true,
                PricePerKg = 30,
                Image = "rejnokObecny.img",
                AvaiableLocations = [FishLocation.Sea]
            },
            new TFish() {
                Name = "Marlin modrý",
                Rarity = FishRarity.Epic,
                Chance = 2,
                Weight = 300.0,
                WeightVar = 200.0, // 100.0 kg – 500.0 kg
                RodLevel = 9,
                IsSea = true,
                PricePerKg = 15,
                Image = "marlinModry.img",
                AvaiableLocations = [FishLocation.Sea]
            },
            new TFish() {
                Name = "Měsíčník modrý",
                Rarity = FishRarity.Mythic,
                Chance = 1,
                Weight = 100.0,
                WeightVar = 80.0, // 20.0 kg – 180.0 kg
                RodLevel = 9,
                IsSea = true,
                PricePerKg = 18,
                Image = "mesicnikModry.img",
                AvaiableLocations = [FishLocation.Sea]
            },
            new TFish() {
                Name = "Manta obrovská",
                Rarity = FishRarity.Mythic,
                Chance = 1,
                Weight = 700.0,
                WeightVar = 500.0, // 200.0 kg – 1200.0 kg
                RodLevel = 10,
                IsSea = true,
                PricePerKg = 12,
                Image = "mantaObrovska.img",
                AvaiableLocations = [FishLocation.DeepSea]
            },
            new TFish() {
                Name = "Žralok bílý",
                Rarity = FishRarity.Mythic,
                Chance = 1,
                Weight = 1000.0,
                WeightVar = 800.0, // 200.0 kg – 1800.0 kg
                RodLevel = 10,
                IsSea = true,
                PricePerKg = 10,
                Image = "zralokBily.img",
                AvaiableLocations = [FishLocation.DeepSea]
            },
            new TFish() {
                Name = "Obří oliheň",
                Rarity = FishRarity.Mythic,
                Chance = 1,
                Weight = 250.0,
                WeightVar = 150.0, // 100.0 kg – 400.0 kg
                RodLevel = 10,
                IsSea = true,
                PricePerKg = 20,
                Image = "obriOlihen.img",
                AvaiableLocations = [FishLocation.DeepSea]
            },




            // Special
            new TFish() {
                Name = "Kraken",
                Rarity = FishRarity.Special,
                Chance = 1,
                Weight = 1000.0,
                WeightVar = 100.0, // 900.0 - 1100.0
                RodLevel = 10,
                IsSea = true,
                PricePerKg = 11,
                Image = "kraken.img",
                AvaiableLocations = [FishLocation.DeepSea]
            },

            new TFish() {
                Name = "Zlatá rybka",
                Rarity = FishRarity.Special,
                Chance = 1,
                Weight = 0.15,
                WeightVar = 0.1,
                RodLevel = 0,
                IsSea = false,
                PricePerKg = 50000,
                Image = "zlataRybka.img",
                AvaiableLocations = [FishLocation.Lake]
            },

            new TFish() {
                Name = "Mořský koník",
                Rarity = FishRarity.Special,
                Chance = 1,
                Weight = 0.05,
                WeightVar = 0.03, // 0.02 kg – 0.08 kg
                RodLevel = 4,
                IsSea = true,
                PricePerKg = 5000,
                Image = "morskyKonik.img",
                AvaiableLocations = [FishLocation.Sea]
            },
            new TFish() {
                Name = "Lvíček ohnivý",
                Rarity = FishRarity.Special,
                Chance = 1,
                Weight = 0.5,
                WeightVar = 0.3, // 0.2 kg – 0.8 kg
                RodLevel = 7,
                IsSea = true,
                PricePerKg = 1000,
                Image = "lvicekOhnivy.img",
                AvaiableLocations = [FishLocation.Sea]
            },
            new TFish() {
                Name = "Latimérie podivná",
                Rarity = FishRarity.Special,
                Chance = 1,
                Weight = 60.0,
                WeightVar = 40.0, // 20.0 kg – 100.0 kg
                RodLevel = 10,
                IsSea = true,
                PricePerKg = 500,
                Image = "latimeriePodivna.img",
                AvaiableLocations = [FishLocation.DeepSea]
            },
            new TFish() {
                Name = "Žralok velrybí",
                Rarity = FishRarity.Special,
                Chance = 1,
                Weight = 1500.0,
                WeightVar = 1000.0, // 500.0 kg – 2500.0 kg
                RodLevel = 10,
                IsSea = true,
                PricePerKg = 8,
                Image = "zralokVelrybi.img",
                AvaiableLocations = [FishLocation.DeepSea]
            }



        };
    }
}
