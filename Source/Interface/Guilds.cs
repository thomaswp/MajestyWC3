using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static War3Api.Common;
using static War3Api.Blizzard;
using WCSharp.Events;
using Source;
using Source.Units;

namespace Source.Interface
{
    public static class Guilds
    {

        private static unit selectedBuilding;

        private static Dictionary<unit, List<UnitAI>> homeMap = new Dictionary<unit, List<UnitAI>>();
        private static Dictionary<player, List<unit>> foodDummyMap = new Dictionary<player, List<unit>>();
        private static List<int> selectAbilityIDs = new List<int>()
        {
            Constants.ABILITY_SELECT_HERO_1,
            Constants.ABILITY_SELECT_HERO_2,
            Constants.ABILITY_SELECT_HERO_3,
            //Constants.ABILITY_SELECT_HERO_2,
        };

        public readonly static List<int> GuildsIDs = new List<int>()
        {
            Constants.UNIT_WARRIORS_BARRACKS,
            Constants.UNIT_RANGERS_HALL,
        };

        public static void Init()
        {
            PlayerUnitEvents.Register(PlayerUnitEvent.PlayerSelectsUnitType, Util.TryAction(() =>
            {
                unit building = GetTriggerUnit();
                //Console.WriteLine($"Start select: {building.GetName()}");
                player player = GetTriggerPlayer();
                if (player != building.GetPlayer()) return;
                if (selectedBuilding != null)
                {
                    SelectUnitRemove(building);
                    return;
                }
                //Console.WriteLine("No currently selected");
                if (!building.IsStructure()) return;
                //Console.WriteLine("Struct");

                int id = building.GetTypeID();
                if (!GuildsIDs.Contains(id)) return;
                //Console.WriteLine("Valid ID");

                selectedBuilding = building;
                PingUnits();
                UpdateFood(player);
                //OwnUnits();
            }, "selecting guild"));

            PlayerUnitEvents.Register(PlayerUnitEvent.PlayerDeselectsUnitType, Util.TryAction(() =>
            {
                unit building = GetTriggerUnit();
                //Console.WriteLine($"Start deselect: {building.GetName()}");
                if (building != selectedBuilding) return;

                //Console.WriteLine("Success");
                player player = building.GetPlayer();
                //ReleaseUnits();
                selectedBuilding = null;
                UpdateFood(player);
            }, "deselecting guild"));

            // Stop a unit training when there's no room, regardless of food cap
            PlayerUnitEvents.Register(PlayerUnitEvent.UnitTypeStartsBeingTrained, Util.TryAction(() =>
            {
                unit building = GetTriggerUnit();
                Console.WriteLine($"Start construct: {building.GetName()}");

                int unitID = building.GetTypeID();
                if (!GuildsIDs.Contains(unitID)) return;
                CheckCancel(building);
            }, "recruiting at guild"));

            PlayerUnitEvents.Register(PlayerUnitEvent.SpellCast, Util.TryAction(() =>
            {
                int index = selectAbilityIDs.IndexOf(GetSpellAbilityId());
                if (index < 0) return;
                Console.WriteLine(GetSpellAbilityId());
                unit guild = GetTriggerUnit();
                SelectHero(guild, index);
            }, "Select"));
        }

        private static void SelectHero(unit guild, int unitNo)
        {
            if (unitNo < 0) return;
            if (!homeMap.TryGetValue(guild, out var ais)) return;
            if (ais.Count <= unitNo) return;
            unit unit = ais[unitNo].Unit;
            player player = guild.GetPlayer();
            SelectUnitForPlayerSingle(unit, player);
            SetCameraPositionLocForPlayer(player, unit.GetLocation());
        }

        private static void CheckCancel(unit building)
        {
            //Console.WriteLine($"Check cancel: {GetHeroCount(building)} vs {GetMaxHeroCount(building)}");
            if (GetHeroCount(building) < GetMaxHeroCount(building)) return;
            IssueImmediateOrderById(building, Constants.ORDER_CANCEL);
            timer timer = CreateTimer();
            int toCancel = 8;
            // Do it again in case there's lots queued...
            TimerStart(timer, 0.03f, true, () =>
            {
                IssueImmediateOrderById(building, Constants.ORDER_CANCEL);
                toCancel--;
                if (toCancel <= 0) DestroyTimer(timer);
            });
            //Console.WriteLine("Canceled building");
        }

        private static void PingUnits()
        {
            if (!homeMap.TryGetValue(selectedBuilding, out var ais)) return;
            ais.ForEach(ai => PingMinimapLocForPlayer(ai.HumanPlayer, ai.Unit.GetLocation(), 1));
        }

        private static void OwnUnits()
        {
            if (!homeMap.TryGetValue(selectedBuilding, out var ais)) return;
            ais.ForEach(ai => SetUnitOwner(ai.Unit, ai.HumanPlayer, false));
        }

        private static void ReleaseUnits()
        {
            if (!homeMap.TryGetValue(selectedBuilding, out var ais)) return;
            ais.ForEach(ai => SetUnitOwner(ai.Unit, ai.AIPlayer, false));
        }

        private static void UpdateFood(player player)
        {
            //Console.WriteLine("Setting food");
            if (selectedBuilding == null)
            {
                SetFood(player, 0);
                // Food cap is set high to allow construction of queued units
                SetFoodCap(player, 100);
                return;
            }

            SetFood(player, GetHeroCount(selectedBuilding));
            SetFoodCap(player, GetMaxHeroCount(selectedBuilding));
        }

        private static int GetMaxHeroCount(unit guild)
        {
            // TODO: Customize
            return 4;
        }

        private static int GetHeroCount(unit guild)
        {
            int food = 0;
            if (homeMap.TryGetValue(guild, out var ais))
            {
                food = ais.Count;
            }

            return food;
        }

        private static void SetFoodCap(player player, int foodCap)
        {
            SetPlayerState(player, PLAYER_STATE_RESOURCE_FOOD_CAP, foodCap);
        }

        private static void SetFood(player player, int food)
        {
            if (!foodDummyMap.TryGetValue(player, out var dummies))
            {
                foodDummyMap[player] = dummies = new List<unit>();
            }
            while (dummies.Count < food)
            {
                unit unit = CreateUnit(player, Constants.UNIT_FOOD_DUMMY, 0, 0, 0);
                if (unit == null)
                {
                    Console.WriteLine("Failed to create food dummy!!");
                }
                ShowUnitHide(unit);
                dummies.Add(unit);
            }
            player neutral = Player(GetBJPlayerNeutralVictim());
            for (int i = 0; i < dummies.Count; i++)
            {
                SetUnitOwner(dummies[i], i < food ? player : neutral, false);
            }
        }

        public static void OnBuildingAttacked(unit home, unit attacker)
        {
            if (homeMap.ContainsKey(home))
            {
                var units = homeMap[home];
                foreach (UnitAI unit in units) unit.OnHomeAttacked(attacker);
            }
        }

        public static void SetHome(UnitAI ai, unit home)
        {
            RemoveAI(ai);
            if (!homeMap.ContainsKey(home))
            {
                homeMap[home] = new List<UnitAI>();
            }
            var units = homeMap[home];
            units.Add(ai);
            if (home == selectedBuilding)
            {
                UpdateFood(selectedBuilding.GetPlayer());
                CheckCancel(selectedBuilding);
            }
            UpdateSelectables(home, units.Count);
        }

        public static void RemoveAI(UnitAI ai)
        {
            if (ai.Home != null)
            {
                var ais = homeMap[ai.Home];
                ais.Remove(ai);
                UpdateSelectables(ai.Home, ais.Count);
            }
        }

        private static void UpdateSelectables(unit home, int count)
        {
            if (home == null)
            {
                Console.WriteLine("Null home!");
                return;
            }
            for (int i = 0; i < selectAbilityIDs.Count; i++)
            {
                int abilityID = selectAbilityIDs[i];
                if (i < count)
                {
                    Console.WriteLine("Adding: " + home.GetName());
                    UnitAddAbility(home, abilityID);
                }
                else
                {
                    Console.WriteLine("Removing: " + home.GetName());
                    UnitRemoveAbility(home, abilityID);
                }
            }
        }

    }
}
