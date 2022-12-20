using System;
using System.Collections.Generic;
using System.Linq;

namespace MapLib
{
    public class Quest
    {
        public string FileVersion { get; set; }
        public string InitFunction { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Player[] Players { get; set; }
        public UnitPattern[] UnitPatterns { get; set; }
        public ForcePattern[] ForcePatterns { get; set; }

        public string Summarize()
        {
            return string.Format("Quest {0} [{1}x{2}]:\nPlayers:\n{3}\n\nUnit Patterns:\n{4}\n\nForcePatterns:\n{5}\n",
                InitFunction,
                Width,
                Height,
                string.Join("\n", Players.Select(p => p.Summarize())),
                string.Join("\n", UnitPatterns.Select(p => p.Summarize())),
                string.Join("\n", ForcePatterns.Select(p => p.Summarize()))
                );
        }
    }

    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int StartingGold { get; set; }
        public int StartingCrystals { get; set; }
        public int[] startingUnitPatternIDs { get; set; }

        public string Summarize()
        {
            return string.Format("{1} (id={0}; {2}): [{3}g, {4}c] has {5}",
                Id, Name, IsActive ? "active" : "inactive", StartingGold, StartingCrystals,
                string.Join(", ", startingUnitPatternIDs)
                );
        }
    }

    public abstract class Pattern
    {
        public string Name { get; set; }
        public int Id1 { get; set; } // unknown
        public int Id2 { get; set; } // unknown
        public string TerrainShortID { get; set; }
        public int Resolution { get; set; }
        public PatternInstance[] Instances { get; set; }

        public virtual string Summarize()
        {

            return string.Format("{0} ({1}, {2}) [terrain={3}, rez={4}]:\n{5}",
                Name, Id1, Id2, TerrainShortID, Resolution,
                string.Join("\n", Instances.Select(i => i.Summarize())));
        }
    }

    public class UnitPattern : Pattern
    {
    }

    public class ForcePattern : Pattern
    {

    }

    public class PatternInstance
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public char[] Tiles { get; set; }

        public string Summarize()
        {
            return string.Format("{1} ({0}): {2}", Id, Name, string.Join(",", Tiles));
        }
    }
}
