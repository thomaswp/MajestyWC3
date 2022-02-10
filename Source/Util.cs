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
        static Random rand = new Random();

        public static TV GetValue<TK, TV>(this IDictionary<TK, TV> dict, TK key, TV defaultValue = default(TV))
        {
            TV value;
            return dict.TryGetValue(key, out value) ? value : defaultValue;
        }

        public static float RandFloat()
        {
            return (float) rand.NextDouble();
        }

        public static float RandBetween(float min, float max)
        {
            return (float)rand.NextDouble() * (max - min) + min;
        }

        public static Action TryAction(Action action, string details)
        {
            return () =>
            {
                try
                {
                    action();
                }
                catch (Exception e)
                {
                    unit unit = GetTriggerUnit();
                    string name = unit == null ? "NONE" : unit.GetName();
                    Console.WriteLine($"Error on {details} for {name}: {e.Message}");
                }
            };
        }

        public static List<unit> ToList(this group group)
        {
            List<unit> units = new List<unit>();
            ForGroup(group, () => units.Add(GetEnumUnit()));
            return units;
        }

        public static string ToXY(this location loc)
        {
            return $"({GetLocationX(loc)}, {GetLocationY(loc)})";
        }

        public static float DistanceTo(this unit me, unit other)
        {
            return DistanceBetweenPoints(GetUnitLoc(me), GetUnitLoc(other));
        }

        public static float DistanceTo(this unit me, location other)
        {
            return DistanceBetweenPoints(GetUnitLoc(me), other);
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

        public static void ChangeGoldBy(this player player, int amount)
        {
            SetPlayerState(player, PLAYER_STATE_RESOURCE_GOLD,
                        GetPlayerState(player, PLAYER_STATE_RESOURCE_GOLD) + amount);
        }

        public static float lerp(float n0, float n1, float p)
        {
            return n0 + (n1 - n0) * p;
        }

        public static float GetSightRange(this unit unit)
        {
            return BlzGetUnitRealField(unit, UNIT_RF_SIGHT_RADIUS);
        }

        public static List<unit> GetVisibleUnits(this unit unit)
        {
            return GetUnitsInRangeOfLocAll(unit.GetSightRange(), unit.GetLocation()).ToList();
        }

        public static bool IsVisibleToPlayer(this unit unit, player player)
        {
            return !IsLocationFoggedToPlayer(unit.GetLocation(), player);
        }

        public static location GetLocation(this unit unit)
        {
            return GetUnitLoc(unit);
        }

        public static int GetHP(this unit unit)
        {
            return MathRound(unit.GetHPFraction() * BlzGetUnitMaxHP(unit));
        }

        public static int GetDamage(this unit unit)
        {
            return MathRound((1 - unit.GetHPFraction()) * BlzGetUnitMaxHP(unit));
        }

        public static float GetHPFraction(this unit unit)
        {
            return GetUnitLifePercent(unit) / 100f;
        }

        public static int GetMana(this unit unit)
        {
            return MathRound(unit.GetManaFraction() * BlzGetUnitMaxMana(unit));
        }

        public static float GetManaFraction(this unit unit)
        {
            return GetUnitManaPercent(unit) / 100f;
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

        public static int RollBountyAward(this unit unit)
        {
            int floor = BlzGetUnitIntegerField(unit, UNIT_IF_GOLD_BOUNTY_AWARDED_BASE);
            int dice = BlzGetUnitIntegerField(unit, UNIT_IF_GOLD_BOUNTY_AWARDED_NUMBER_OF_DICE);
            int sides = BlzGetUnitIntegerField(unit, UNIT_IF_GOLD_BOUNTY_AWARDED_SIDES_PER_DIE);

            int bounty = floor;
            for (int i = 0; i < sides; i++)
            {
                bounty += GetRandomInt(1, sides);
            }
            return bounty;
        }

        public static bool IsCastle(this unit unit)
        {
            // TODO: All levels
            return unit.GetTypeID() == Constants.UNIT_CASTLE_LEVEL_1;
        }
    }
}
