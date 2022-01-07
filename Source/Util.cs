﻿using System;
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

        public static float DistanceTo(this unit me, location other)
        {
            return DistanceBetweenPoints(GetUnitLoc(me), other);
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

        public static float GetSightRange(this unit unit)
        {
            return BlzGetUnitRealField(unit, UNIT_RF_SIGHT_RADIUS);
        }

        public static location GetLocation(this unit unit)
        {
            return GetUnitLoc(unit);
        }

        public static int GetHP(this unit unit)
        {
            return (int) Math.Round(GetUnitLifePercent(unit) * BlzGetUnitMaxHP(unit));
        }

        public static bool IsStructure(this unit unit)
        {
            return IsUnitIdType(GetUnitTypeId(unit), UNIT_TYPE_STRUCTURE);
        }

        public static bool IsDead(this unit unit)
        {
            return IsUnitDeadBJ(unit);
        }

        public static string GetName(this unit unit)
        {
            return GetUnitName(unit);
        }

        public static int GetTypeID(this unit unit)
        {
            return GetUnitTypeId(unit);
        }

        public static player GetPlayer(this unit unit)
        {
            return GetOwningPlayer(unit);
        }

        public static void OrderMoveTo(this unit unit, location loc)
        {
            IssuePointOrderByIdLoc(unit, Constants.ORDER_MOVE, loc);
        }
    }
}
