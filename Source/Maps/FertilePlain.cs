using MapLib;

namespace Source.Maps
{
    public class FertilePlain : Map
    {
        public override Quest getQuest()
        {
            var quest = new Quest
            {
                FileVersion = "RGM6",
                InitFunction = "FERTILE_PLAIN",
                Width = 128,
                Height = 128,
                Players = new Player[]
                {
                    new Player
                    {
                        Id = 0,
                        Name = "Human Player",
                        IsActive = true,
                        StartingGold = 40000,
                        StartingCrystals = 6000,
                        startingUnitPatternIDs = new int[]
                        {
                            0
                        }
                    },
                    new Player
                    {
                        Id = 1,
                        Name = "none",
                        IsActive = false,
                        StartingGold = 0,
                        StartingCrystals = 0,
                        startingUnitPatternIDs = new int[]
                        {
                        }
                    },
                    new Player
                    {
                        Id = 2,
                        Name = "none",
                        IsActive = false,
                        StartingGold = 0,
                        StartingCrystals = 0,
                        startingUnitPatternIDs = new int[]
                        {
                        }
                    },
                    new Player
                    {
                        Id = 3,
                        Name = "none",
                        IsActive = false,
                        StartingGold = 0,
                        StartingCrystals = 0,
                        startingUnitPatternIDs = new int[]
                        {
                        }
                    },
                    new Player
                    {
                        Id = 4,
                        Name = "none",
                        IsActive = false,
                        StartingGold = 0,
                        StartingCrystals = 0,
                        startingUnitPatternIDs = new int[]
                        {
                        }
                    },
                    new Player
                    {
                        Id = 5,
                        Name = "none",
                        IsActive = false,
                        StartingGold = 0,
                        StartingCrystals = 0,
                        startingUnitPatternIDs = new int[]
                        {
                        }
                    },
                    new Player
                    {
                        Id = 6,
                        Name = "none",
                        IsActive = false,
                        StartingGold = 0,
                        StartingCrystals = 0,
                        startingUnitPatternIDs = new int[]
                        {
                        }
                    },
                    new Player
                    {
                        Id = 7,
                        Name = "Monsters",
                        IsActive = true,
                        StartingGold = 0,
                        StartingCrystals = 0,
                        startingUnitPatternIDs = new int[]
                        {
                            1,
                            2,
                            3,
                            4
                        }
                    }
                },
                            UnitPatterns = new UnitPattern[]
                {
                    new UnitPattern
                    {
                        Name = "Fertile Plain lairs #1",
                        Id1 = 0,
                        Id2 = 9,
                        TerrainShortID = "B.B.",
                        Resolution = 5,
                        Instances = new PatternInstance[]
                        {
                            new PatternInstance
                            {
                                Id = "BBB1",
                                Name = "Dark Castle 1",
                                Tiles = new char[]
                                {
                                    'M'
                                }
                            },
                            new PatternInstance
                            {
                                Id = "BBB1",
                                Name = "Dark Castle 1",
                                Tiles = new char[]
                                {
                                    'F'
                                }
                            }
                        }
                    },
                    new UnitPattern
                    {
                        Name = "Fertile Plain Lairs #2",
                        Id1 = 0,
                        Id2 = 9,
                        TerrainShortID = "B.B.",
                        Resolution = 5,
                        Instances = new PatternInstance[]
                        {
                            new PatternInstance
                            {
                                Id = "BBA1",
                                Name = "Animal Den",
                                Tiles = new char[]
                                {
                                    'M'
                                }
                            }
                        }
                    },
                    new UnitPattern
                    {
                        Name = "Fertile Plains lairs #3",
                        Id1 = 0,
                        Id2 = 9,
                        TerrainShortID = "B.B.",
                        Resolution = 5,
                        Instances = new PatternInstance[]
                        {
                            new PatternInstance
                            {
                                Id = "BBB2",
                                Name = "Dark Castle 2",
                                Tiles = new char[]
                                {
                                    'M'
                                }
                            },
                            new PatternInstance
                            {
                                Id = "BBB2",
                                Name = "Dark Castle 2",
                                Tiles = new char[]
                                {
                                    'V'
                                }
                            }
                        }
                    },
                    new UnitPattern
                    {
                        Name = "Fertile Plains lairs #4",
                        Id1 = 0,
                        Id2 = 9,
                        TerrainShortID = "B.B.",
                        Resolution = 5,
                        Instances = new PatternInstance[]
                        {
                            new PatternInstance
                            {
                                Id = "BBB4",
                                Name = "Dark Castle 4",
                                Tiles = new char[]
                                {
                                    'M'
                                }
                            },
                            new PatternInstance
                            {
                                Id = "BBB3",
                                Name = "Dark Castle 3",
                                Tiles = new char[]
                                {
                                    'I'
                                }
                            }
                        }
                    },
                    new UnitPattern
                    {
                        Name = "Fertile plain Start",
                        Id1 = 0,
                        Id2 = 1,
                        TerrainShortID = "B.B.",
                        Resolution = 5,
                        Instances = new PatternInstance[]
                        {
                            new PatternInstance
                            {
                                Id = "ABJ2",
                                Name = "Palace Level 2",
                                Tiles = new char[]
                                {
                                    'M'
                                }
                            },
                            new PatternInstance
                            {
                                Id = "ABe1",
                                Name = "General Housing",
                                Tiles = new char[]
                                {
                                    'Q'
                                }
                            },
                            new PatternInstance
                            {
                                Id = "ABe1",
                                Name = "General Housing",
                                Tiles = new char[]
                                {
                                    'R'
                                }
                            },
                            new PatternInstance
                            {
                                Id = "ABe1",
                                Name = "General Housing",
                                Tiles = new char[]
                                {
                                    'L'
                                }
                            },
                            new PatternInstance
                            {
                                Id = "ABE1",
                                Name = "Guardhouse",
                                Tiles = new char[]
                                {
                                    'U'
                                }
                            }
                        }
                    }
                },
                            ForcePatterns = new ForcePattern[]
                {
                    new ForcePattern
                    {
                        Name = "Fertile Plain",
                        Id1 = 0,
                        Id2 = 0,
                        TerrainShortID = "NONE",
                        Resolution = 6,
                        Instances = new PatternInstance[]
                        {
                            new PatternInstance
                            {
                                Id = "Fert",
                                Name = "Fertile plain Start",
                                Tiles = new char[]
                                {
                                    'H',
                                    'L',
                                    'M',
                                    'N',
                                    'R'
                                }
                            },
                            new PatternInstance
                            {
                                Id = "Fera",
                                Name = "Fertile Plain lairs #1",
                                Tiles = new char[]
                                {
                                    'A',
                                    'B',
                                    'F',
                                    'G'
                                }
                            },
                            new PatternInstance
                            {
                                Id = "Ferb",
                                Name = "Fertile Plain Lairs #2",
                                Tiles = new char[]
                                {
                                    'D',
                                    'E',
                                    'I',
                                    'J'
                                }
                            },
                            new PatternInstance
                            {
                                Id = "Ferc",
                                Name = "Fertile Plains lairs #3",
                                Tiles = new char[]
                                {
                                    'S',
                                    'T',
                                    'X',
                                    'Y'
                                }
                            },
                            new PatternInstance
                            {
                                Id = "Ferd",
                                Name = "Fertile Plains lairs #4",
                                Tiles = new char[]
                                {
                                    'P',
                                    'Q',
                                    'U',
                                    'V'
                                }
                            }
                        }
                    }
                }
            };
            return quest;
        }
    }
}
