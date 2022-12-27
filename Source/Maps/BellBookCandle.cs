using MapLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source.Maps
{
    public class BellBookCandle : Map
    {
        public override Quest getQuest()
        {
            var quest = new Quest
            {
                FileVersion = "RGM6",
                InitFunction = "BELL_BOOK_CANDLE",
                Width = 128,
                Height = 128,
                Players = new List<Player>
                {
                    new Player
                    {
                        Id = 0,
                        Name = "Human Player",
                        IsActive = true,
                        StartingGold = 15000,
                        StartingCrystals = 5000,
                        StartingUnitPatternIDs = new int[]
                        {
                            3,
                            5,
                            4
                        }
                    },
                    new Player
                    {
                        Id = 1,
                        Name = "none",
                        IsActive = false,
                        StartingGold = 0,
                        StartingCrystals = 0,
                        StartingUnitPatternIDs = new int[]
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
                        StartingUnitPatternIDs = new int[]
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
                        StartingUnitPatternIDs = new int[]
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
                        StartingUnitPatternIDs = new int[]
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
                        StartingUnitPatternIDs = new int[]
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
                        StartingUnitPatternIDs = new int[]
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
                        StartingUnitPatternIDs = new int[]
                        {
                            0,
                            1,
                            2
                        }
                    }
                },
                            UnitPatterns = new List<UnitPattern>
                {
                    new UnitPattern
                    {
                        Name = "BBC startup #3",
                        Id1 = 0,
                        Id2 = 9,
                        TerrainShortID = "B.B.",
                        Resolution = 4,
                        Instances = new List<PatternInstance>
                        {
                            new PatternInstance
                            {
                                Id = "ABL1",
                                Name = "Trading Post",
                                Tiles = new char[]
                                {
                                    'M'
                                }
                            }
                        }
                    },
                    new UnitPattern
                    {
                        Name = "B.B.C. startup",
                        Id1 = 1,
                        Id2 = 1,
                        TerrainShortID = "#BBC",
                        Resolution = 10,
                        Instances = new List<PatternInstance>
                        {
                            new PatternInstance
                            {
                                Id = "ABJ1",
                                Name = "Palace",
                                Tiles = new char[]
                                {
                                    'P'
                                }
                            },
                            new PatternInstance
                            {
                                Id = "ABC1",
                                Name = "Blacksmith",
                                Tiles = new char[]
                                {
                                    'H'
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
                            },
                            new PatternInstance
                            {
                                Id = "ABX1",
                                Name = "Rogues Guild",
                                Tiles = new char[]
                                {
                                    'O'
                                }
                            }
                        }
                    },
                    new UnitPattern
                    {
                        Name = "Treasure Trove #1",
                        Id1 = 0,
                        Id2 = 9,
                        TerrainShortID = "#Arr",
                        Resolution = 3,
                        Instances = new List<PatternInstance>
                        {
                            new PatternInstance
                            {
                                Id = "BBt4",
                                Name = "Treasure Chest",
                                Tiles = new char[]
                                {
                                    'M'
                                }
                            },
                            new PatternInstance
                            {
                                Id = "BBt1",
                                Name = "Treasure Chest",
                                Tiles = new char[]
                                {
                                    'B'
                                }
                            },
                            new PatternInstance
                            {
                                Id = "BBt1",
                                Name = "Treasure Chest",
                                Tiles = new char[]
                                {
                                    'H'
                                }
                            },
                            new PatternInstance
                            {
                                Id = "BBt2",
                                Name = "Treasure Chest",
                                Tiles = new char[]
                                {
                                    'I'
                                }
                            },
                            new PatternInstance
                            {
                                Id = "BBt3",
                                Name = "Treasure Chest",
                                Tiles = new char[]
                                {
                                    'Q'
                                }
                            }
                        }
                    },
                    new UnitPattern
                    {
                        Name = "candle location",
                        Id1 = 0,
                        Id2 = 9,
                        TerrainShortID = "Barr",
                        Resolution = 4,
                        Instances = new List<PatternInstance>
                        {
                            new PatternInstance
                            {
                                Id = "BBL1",
                                Name = "Ruined Keep",
                                Tiles = new char[]
                                {
                                    'M'
                                }
                            },
                            new PatternInstance
                            {
                                Id = "BVZ1",
                                Name = "Zombie",
                                Tiles = new char[]
                                {
                                    'A'
                                }
                            },
                            new PatternInstance
                            {
                                Id = "BVZ1",
                                Name = "Zombie",
                                Tiles = new char[]
                                {
                                    'E'
                                }
                            },
                            new PatternInstance
                            {
                                Id = "BVT1",
                                Name = "Skeleton",
                                Tiles = new char[]
                                {
                                    'W'
                                }
                            }
                        }
                    },
                    new UnitPattern
                    {
                        Name = "Lone Inn",
                        Id1 = 0,
                        Id2 = 9,
                        TerrainShortID = "#Arr",
                        Resolution = 5,
                        Instances = new List<PatternInstance>
                        {
                            new PatternInstance
                            {
                                Id = "ABF1",
                                Name = "Inn",
                                Tiles = new char[]
                                {
                                    'S'
                                }
                            },
                            new PatternInstance
                            {
                                Id = "ABE1",
                                Name = "Guardhouse",
                                Tiles = new char[]
                                {
                                    'G'
                                }
                            }
                        }
                    },
                    new UnitPattern
                    {
                        Name = "book location",
                        Id1 = 0,
                        Id2 = 9,
                        TerrainShortID = "Barr",
                        Resolution = 4,
                        Instances = new List<PatternInstance>
                        {
                            new PatternInstance
                            {
                                Id = "BBM1",
                                Name = "Ruined Shrine",
                                Tiles = new char[]
                                {
                                    'M'
                                }
                            },
                            new PatternInstance
                            {
                                Id = "BVO1",
                                Name = "Medusa",
                                Tiles = new char[]
                                {
                                    'Y'
                                }
                            }
                        }
                    },
                    new UnitPattern
                    {
                        Name = "Bell location",
                        Id1 = 0,
                        Id2 = 9,
                        TerrainShortID = "Barr",
                        Resolution = 4,
                        Instances = new List<PatternInstance>
                        {
                            new PatternInstance
                            {
                                Id = "BBK1",
                                Name = "Ruined Altar",
                                Tiles = new char[]
                                {
                                    'M'
                                }
                            },
                            new PatternInstance
                            {
                                Id = "BVJ1",
                                Name = "Giant Spider",
                                Tiles = new char[]
                                {
                                    'A'
                                }
                            },
                            new PatternInstance
                            {
                                Id = "BVJ1",
                                Name = "Giant Spider",
                                Tiles = new char[]
                                {
                                    'Y'
                                }
                            }
                        }
                    }
                },
                            ForcePatterns = new List<ForcePattern>
                {
                    new ForcePattern
                    {
                        Name = "bell book candle",
                        Id1 = 0,
                        Id2 = 0,
                        TerrainShortID = "NONE",
                        Resolution = 4,
                        Instances = new List<PatternInstance>
                        {
                            new PatternInstance
                            {
                                Id = "Bell",
                                Name = "Bell location",
                                Tiles = new char[]
                                {
                                    'A',
                                    'B',
                                    'C',
                                    'D',
                                    'E',
                                    'F',
                                    'J',
                                    'K',
                                    'O',
                                    'P',
                                    'T',
                                    'U',
                                    'V',
                                    'W',
                                    'X',
                                    'Y'
                                }
                            },
                            new PatternInstance
                            {
                                Id = "book",
                                Name = "book location",
                                Tiles = new char[]
                                {
                                    'A',
                                    'B',
                                    'C',
                                    'D',
                                    'E',
                                    'F',
                                    'J',
                                    'K',
                                    'O',
                                    'P',
                                    'T',
                                    'U',
                                    'V',
                                    'W',
                                    'X',
                                    'Y'
                                }
                            },
                            new PatternInstance
                            {
                                Id = "cand",
                                Name = "candle location",
                                Tiles = new char[]
                                {
                                    'G',
                                    'I',
                                    'Q',
                                    'S'
                                }
                            },
                            new PatternInstance
                            {
                                Id = "B.B.",
                                Name = "B.B.C. startup",
                                Tiles = new char[]
                                {
                                    'M'
                                }
                            },
                            new PatternInstance
                            {
                                Id = "Lone",
                                Name = "Lone Inn",
                                Tiles = new char[]
                                {
                                    'G',
                                    'H',
                                    'I',
                                    'L',
                                    'N',
                                    'Q',
                                    'R',
                                    'S'
                                }
                            },
                            new PatternInstance
                            {
                                Id = "BBC ",
                                Name = "BBC startup #3",
                                Tiles = new char[]
                                {
                                    'A',
                                    'B',
                                    'C',
                                    'D',
                                    'E',
                                    'F',
                                    'J',
                                    'K',
                                    'O',
                                    'P',
                                    'T',
                                    'U',
                                    'V',
                                    'W',
                                    'X',
                                    'Y'
                                }
                            },
                            new PatternInstance
                            {
                                Id = "Trea",
                                Name = "Treasure Trove #1",
                                Tiles = new char[]
                                {
                                    'A',
                                    'E',
                                    'U',
                                    'Y'
                                }
                            },
                            new PatternInstance
                            {
                                Id = "Trea",
                                Name = "Treasure Trove #1",
                                Tiles = new char[]
                                {
                                    'A',
                                    'E',
                                    'U',
                                    'Y'
                                }
                            },
                            new PatternInstance
                            {
                                Id = "Trea",
                                Name = "Treasure Trove #1",
                                Tiles = new char[]
                                {
                                    'G',
                                    'H',
                                    'I',
                                    'L',
                                    'N',
                                    'Q',
                                    'R',
                                    'S'
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
