using System;
using System.Linq;

namespace MapLib
{
    public class Quest
    {
        public Player[] players;
        public UnitPattern[] unitPatterns;
        public ForcePattern[] forcePatterns;

        public string Summarize()
        {
            return "";
        }
    }

    public class Player
    {
        public int id;
        public string name;
        public bool isActive;
        public int startingGold;
        public int startingCrystals;
        public int[] startingUnitPatternIDs;

        public string Summarize()
        {
            return string.Format("{1} (id={0}; {2}): [{3}g, {4}c] has {5}",
                id, name, isActive ? "active" : "inactive", startingGold, startingCrystals,
                string.Join(", ", startingUnitPatternIDs)
                );
        }
    }

    public abstract class Pattern
    {
        public string name;
        public int id1, id2; //unknown
        public string terrainShortID;
        public int resolution;
        public PatternInstance[] instances;

        public virtual string Summarize()
        {

            return string.Format("{0} ({1}, {2}) [terrain={3}, rez={4}]:\n{5}",
                name, id1, id2, terrainShortID, resolution,
                string.Join("\n", instances.Select(i => i.Summarize())));
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
        public string id;
        public string name;
        public char[] tiles;

        public string Summarize()
        {
            return string.Format("{1} ({0}): {2}", id, name, string.Join(",", tiles));
        }
    }
}
