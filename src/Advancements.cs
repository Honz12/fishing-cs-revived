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
        private class Definition
        {
            public required string Id;
            public required string Name;
            public required string Description;
            public required string IconName;
            public required Func<PlayerData, bool> Condition;
        }

        private static bool HasCaughtRarity(PlayerData d, FishRarity rarity)
        {
            return d.UnlockedFishIds.Any(id => FishData.fishes[id].Rarity == rarity);
        }

        private static bool HasCaughtKraken(PlayerData d)
        {
            return d.UnlockedFishIds.Any(id => FishData.fishes[id].Name == "Kraken");
        }

        private static readonly Definition[] Definitions =
        {
            new Definition {
                Id = "prvniUlovek",
                Name = "1. Úlovek!",
                Description = "Chyť svou první rybu.",
                IconName = "prvniUlovek.img",
                Condition = d => d.Inventory.Count > 0
            },
            new Definition {
                Id = "vzacnyUlovek",
                Name = "Něco vzácnějšího",
                Description = "Chyť rybu neobyčejné vzácnosti.",
                IconName = "vzacnyUlovek.img",
                Condition = d => HasCaughtRarity(d, FishRarity.Rare)
            },
            new Definition {
                Id = "epickyUlovek",
                Name = "Epický úlovek",
                Description = "Chyť epickou rybu.",
                IconName = "epickyUlovek.img",
                Condition = d => HasCaughtRarity(d, FishRarity.Epic)
            },
            new Definition {
                Id = "mytickyUlovek",
                Name = "Mýtický úlovek",
                Description = "Chyť mytickou rybu.",
                IconName = "mytickyUlovek.img",
                Condition = d => HasCaughtRarity(d, FishRarity.Mythic)
            },
            new Definition {
                Id = "chytKrakena",
                Name = "Co to je?!",
                Description = "Chyť Krakena.",
                IconName = "chytKrakena.img",
                Condition = d => HasCaughtKraken(d)
            },
            new Definition {
                Id = "desetUlovku",
                Name = "Rozjetá jízda",
                Description = "Chyť 10 ryb.",
                IconName = "desetUlovku.img",
                Condition = d => d.FishCaughtCount >= 10
            },
            new Definition {
                Id = "stoUlovku",
                Name = "Stovka!",
                Description = "Chyť 100 ryb.",
                IconName = "stoUlovku.img",
                Condition = d => d.FishCaughtCount >= 100
            },
            new Definition {
                Id = "tisicUlovku",
                Name = "Tisícovka",
                Description = "Chyť 1000 ryb.",
                IconName = "tisicUlovku.img",
                Condition = d => d.FishCaughtCount >= 1000
            },
            new Definition {
                Id = "objevitel",
                Name = "Objevitel",
                Description = "Objev 10 druhů ryb.",
                IconName = "objevitel.img",
                Condition = d => d.UnlockedFishIds.Count >= 10
            },
            new Definition {
                Id = "znalecVod",
                Name = "Znalec vod",
                Description = "Objev 25 druhů ryb.",
                IconName = "znalecVod.img",
                Condition = d => d.UnlockedFishIds.Count >= 25
            },
            new Definition {
                Id = "dokonalaSbirka",
                Name = "Dokonalá sbírka",
                Description = "Objev všechny druhy ryb.",
                IconName = "dokonalaSbirka.img",
                Condition = d => d.UnlockedFishIds.Count >= FishData.fishes.Length
            },
            new Definition {
                Id = "prvniDesettisic",
                Name = "První desetitisícovka",
                Description = "Celkem vydělej 10 000 Kč.",
                IconName = "prvniDesettisic.img",
                Condition = d => d.TotalMoneyEarned >= 10_000
            },
            new Definition {
                Id = "stoTisic",
                Name = "Stotisícová jízda",
                Description = "Celkem vydělej 100 000 Kč.",
                IconName = "stoTisic.img",
                Condition = d => d.TotalMoneyEarned >= 100_000
            },
            new Definition {
                Id = "milionar",
                Name = "Milionář",
                Description = "Celkem vydělej 1 000 000 Kč.",
                IconName = "milionar.img",
                Condition = d => d.TotalMoneyEarned >= 1_000_000
            },
            new Definition {
                Id = "poradnyPrut",
                Name = "Konečně pořádný prut!",
                Description = "Upgraduj poprvé svůj prut.",
                IconName = "poradnyPrut.img",
                Condition = d => d.RodLevel >= 1
            },
            new Definition {
                Id = "polovicniPrut",
                Name = "Napůl cesty",
                Description = "Vylepši prut na úroveň 5.",
                IconName = "polovicniPrut.img",
                Condition = d => d.RodLevel >= 4
            },
            new Definition {
                Id = "silnaLod",
                Name = "Pevná loď",
                Description = "Vylepši loď na úroveň 3.",
                IconName = "silnaLod.img",
                Condition = d => d.InventorySize >= 2
            },
            new Definition {
                Id = "domaNejlip",
                Name = "Všude dobře, doma nejlíp!",
                Description = "Kup si svůj první dům.",
                IconName = "domaNejlip.img",
                Condition = d => d.HouseLevel >= 1
            },
            new Definition {
                Id = "vydalSeNaMore",
                Name = "Na moře!",
                Description = "Odemkni lokaci Moře.",
                IconName = "vydalSeNaMore.img",
                Condition = d => d.LocationUpgrade >= 1
            },
            new Definition {
                Id = "hlubinyMore",
                Name = "Do hlubin",
                Description = "Odemkni lokaci Hluboké moře.",
                IconName = "hlubinyMore.img",
                Condition = d => d.LocationUpgrade >= 2
            },
            new Definition {
                Id = "tatuvTelefonat",
                Name = "Tátův telefonát.",
                Description = "Táta ti zavolal.",
                IconName = "tatuvTelefonat.img",
                Condition = d => d.HouseLevel >= 4
            },
            new Definition {
                Id = "max",
                Name = "Max!",
                Description = "Koupil jsi všechny upgrady.",
                IconName = "max.img",
                Condition = d => d.HouseLevel >= 4 && d.RodLevel == 10 && d.InventorySize >= 4
            },
        };

        public static int AdvancementCount => Definitions.Length;

        /// <summary>
        /// Checks if the player has unlocked any new advancements based on their current data.
        /// If not returns null, otherwise returns the Advancement object that was unlocked.
        /// </summary>
        /// <param name="playerData"></param>
        /// <returns></returns>
        public static Advancement? CheckForNewAdvancements(PlayerData playerData)
        {
            foreach (Definition definition in Definitions)
            {
                if (!playerData.UnlockedAdvancementIds.Contains(definition.Id) && definition.Condition(playerData))
                {
                    playerData.UnlockedAdvancementIds.Add(definition.Id);
                    return new Advancement()
                    {
                        Name = definition.Name,
                        Description = definition.Description,
                        IconName = definition.IconName,
                        Id = definition.Id
                    };
                }
            }

            return null;
        }

        public static void UnlockAll(PlayerData playerData)
        {
            foreach (Definition definition in Definitions)
            {
                if (!playerData.UnlockedAdvancementIds.Contains(definition.Id))
                {
                    playerData.UnlockedAdvancementIds.Add(definition.Id);
                    playerData.Advancements.Add(new Advancement()
                    {
                        Name = definition.Name,
                        Description = definition.Description,
                        IconName = definition.IconName,
                        Id = definition.Id
                    });
                }
            }
        }

        public static bool UnlockById(PlayerData playerData, string id)
        {
            foreach (Definition definition in Definitions)
            {
                if (definition.Id == id)
                {
                    if (!playerData.UnlockedAdvancementIds.Contains(definition.Id))
                    {
                        playerData.UnlockedAdvancementIds.Add(definition.Id);
                        playerData.Advancements.Add(new Advancement()
                        {
                            Name = definition.Name,
                            Description = definition.Description,
                            IconName = definition.IconName,
                            Id = definition.Id
                        });
                    }
                    return true;
                }
            }

            return false;
        }
    }
}
