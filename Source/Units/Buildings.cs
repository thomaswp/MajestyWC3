using Source.Interface;
using Source.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCSharp.Events;
using static War3Api.Common;
using static War3Api.Blizzard;

namespace Source.Units
{
    public class Building
    {
        public unit Unit { get; private set; }

        public int Gold { get; set; }

        public BuildingInfo Info;

        private timer upgradeTimer;
        private int targetMaxHP;

        public Building(unit unit)
        {
            Unit = unit;
            UpdateInfo();
        }

        private void UpdateInfo()
        {
            Info = Unit.GetTypeID();
        }

        private int GetUpgradeMaxHP()
        {
            // TODO create safe spawn point? Or precompute at start?
            unit unit = CreateUnit(Player(GetBJPlayerNeutralVictim()), Unit.GetTypeID(), 0, 0, 0);
            if (unit != null)
            {
                int maxHp = BlzGetUnitMaxHP(unit);
                RemoveUnit(unit);
                return maxHp;
            }
            Console.WriteLine("Could not create dummy");
            return MathRound(BlzGetUnitMaxHP(Unit) * 1.3f);
        }

        public void StartUpgrade()
        {
            // Max be easier and less error prone to just remove health, rather than
            // trying to early add max hp and later remove it...

            UpdateInfo();
            //Console.WriteLine("Starting upgrade: " + Unit.GetName());
            targetMaxHP = GetUpgradeMaxHP();
            int startHP = Unit.GetHP();
            float startFrac = Math.Min(startHP / targetMaxHP, 0.99f);
            Console.WriteLine($"{startHP} / {targetMaxHP}: {startFrac}%");
            BlzSetUnitMaxHP(Unit, targetMaxHP);
            BlzSetUnitRealField(Unit, UNIT_RF_HP, startHP);
            upgradeTimer = CreateTimer();
            TimerStart(upgradeTimer, 0.1f, true, () =>
            {
                float fraction = (Unit.GetHPFraction() - startFrac) / (1 - startFrac);
                fraction = Math.Max(0, Math.Min(1, fraction));
                Console.WriteLine($"{Unit.GetHPFraction() - startFrac} / {1 - startFrac} = {fraction}");
                UnitSetUpgradeProgress(Unit, MathRound(fraction * 100 - 0.4f));
            });
            UpdateInfo();
        }

        public void FinishUpgrade()
        {
            DestroyTimer(upgradeTimer);
            // Set MaxHP back, since we added it earlier...
            BlzSetUnitMaxHP(Unit, targetMaxHP);
        }

        internal void AddIncome()
        {
            Gold += Info.Income;
        }
    }

    public static class Buildings
    {

        public static Dictionary<unit, Building> buildingMap = new();

        public static void Init()
        {
            PlayerUnitEvents.Register(PlayerUnitEvent.UnitTypeFinishesBeingConstructed, Util.TryAction(() =>
            {
                // TODO: Handle upgrades?
                unit unit = GetTriggerUnit();
                if (TryRegister(unit))
                {
                    player owner = unit.GetPlayer();
                    SetUnitOwner(unit, owner.GetHumanForAI(), false);
                }
            }, "registering building"));

            PlayerUnitEvents.Register(PlayerUnitEvent.UnitTypeStartsBeingConstructed, Util.TryAction(() =>
            {
                unit unit = GetTriggerUnit();
                if (!unit.IsStructure()) return;
                player owner = unit.GetPlayer();
                if (owner != owner.GetHumanForAI()) return;
                player computer = owner.GetAIForHuman();
                SetUnitOwner(unit, computer, false);

            }, "start construction"));

            PlayerUnitEvents.Register(PlayerUnitEvent.UnitTypeStartsUpgrade, Util.TryAction(() =>
            {
                unit unit = GetTriggerUnit();
                Building building = Get(unit);
                if (building == null) return;
                building.StartUpgrade();

            }, "start upgrade"));

            PlayerUnitEvents.Register(PlayerUnitEvent.UnitTypeFinishesUpgrade, Util.TryAction(() =>
            {
                unit unit = GetTriggerUnit();
                Building building = Get(unit);
                if (building == null) return;
                building.FinishUpgrade();

            }, "finish upgrade"));

            PlayerUnitEvents.Register(PlayerUnitEvent.UnitTypeDies, Util.TryAction(() =>
            {
                unit building = GetTriggerUnit();
                TryRemove(building);
            }, "registering building"));

            TimerStart(CreateTimer(), 5, true, Util.TryAction(() =>
            {
                // TODO: Do all castles
                foreach (var castle in GetUnitsOfTypeIdAll(Constants.UNIT_CASTLE_LEVEL_1).ToList())
                {
                    player player = castle.GetPlayer();
                    int heroes = UnitAI.GetHeroCount(player);
                    int houses = GetUnitsOfPlayerAndTypeId(player, Constants.UNIT_HOUSE).Count();

                    if ((heroes + 3) / 4 > houses)
                    {
                        CreateHouse(player, castle);
                    }

                }
            }, "Creating houses"));

            TimerStart(CreateTimer(), 30, true, Util.TryAction(() =>
            {
                foreach (var building in buildingMap.Values)
                {
                    building.AddIncome();
                }
            }, "Adding income"));
        }

        private static void CreateHouse(player player, unit castle)
        {
            var castleLoc = castle.GetLocation();
            float castleX = castleLoc.X(), castleY = castleLoc.Y();
            
            for (int radius = 500; radius < 5000; radius += 200)
            {
                float offset = Util.RandBetween(0, 360);
                for (int angleBase = 0; angleBase < 360; angleBase += 10)
                {
                    float angle = angleBase + offset;
                    angle *= (float) Math.PI / 180;

                    float x = castleX + radius * (float)Math.Cos(angle);
                    float y = castleY + radius * (float)Math.Sin(angle);
                    location location = Location(x, y);

                    if (GetUnitsInRangeOfLocAll(400, location).Any()) continue;
                    if (!IsVisibleToPlayer(x, y, player)) continue;

                    CreateUnit(player, Constants.UNIT_HOUSE, x, y, 0);
                    return;
                }
            }

            Console.WriteLine("Cannot build house!");
        }

        public static void AddTax(unit shop, int pretax)
        {
            ShopInfo info = shop.GetTypeID();
            if (info == null) return;
            ChangeGold(shop, MathRound(pretax * info.TaxRate));
        }

        public static bool CanBeTaxed(this unit building)
        {
            var b = Get(building);
            return b != null && b.Info.CanBeTaxed;
        }

        public static Building Get(unit unit)
        {
            if (buildingMap.TryGetValue(unit, out var building)) return building;
            return null;
        }

        public static bool TryRegister(unit building)
        {
            if (!BuildingInfo.Map.ContainsKey(building.GetTypeID())) return false;
            buildingMap.Add(building, new Building(building));
            return true;
        }

        static bool TryRemove(unit unit)
        {
            return buildingMap.Remove(unit);
        }

        public static int GetGold(unit building)
        {
            Building b = Get(building);
            if (b != null)
            {
                return b.Gold;
            }
            else
            {
                Console.WriteLine($"Unregistered building: {building.GetName()}");
                return 0;
            }
        }

        public static void ChangeGold(unit building, int amount)
        {
            Building b = Get(building);
            if (b != null)
            {
                b.Gold += amount;
                //Console.WriteLine($"Adding {taxes} from {untaxed} to {building.GetName()}");
            }
            else
            {
                Console.WriteLine($"Unregistered building: {building.GetName()}");
            }
        }
    }
}
