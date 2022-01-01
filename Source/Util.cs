using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static War3Api.Common;
using static War3Api.Blizzard;

namespace Source
{
    public static class Util
    {
        public static List<unit> ToList(this group group)
        {
            List<unit> units = new List<unit>();
            ForGroup(group, () => units.Add(GetEnumUnit()));
            return units;
        }

        public static float DistanceTo(this unit me, unit other)
        {
            return DistanceBetweenPoints(GetUnitLoc(me), GetUnitLoc(other));
        }

        public static string Name(this unit unit)
        {
            return GetUnitName(unit);
        }

        public static player GetAIForHuman(this player player)
        {
            int id = GetPlayerId(player);
            if (id % 2 != 0) return player;
            return Player(id + 1);
        }

        public static player GetHumanForAI(this player player)
        {
            int id = GetPlayerId(player);
            if (id % 2 != 1) return player;
            return Player(id - 1);
        }

        public static float lerp(float n0, float n1, float p)
        {
            return n0 + (n1 - n0) * p;
        }
    }
}
