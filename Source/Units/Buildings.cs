using Source.Interface;
using Source.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCSharp.Events;
using static War3Api.Common;

namespace Source.Units
{
    public class Building
    {
        public unit Unit { get; private set; }

        public int Gold { get; set; }

        public Building(unit unit)
        {
            Unit = unit;
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
                if (!unit.IsStructure()) return;
                Console.WriteLine("Starting upgrade: " + unit.GetName());

            }, "start upgrade"));

            PlayerUnitEvents.Register(PlayerUnitEvent.UnitTypeDies, Util.TryAction(() =>
            {
                unit building = GetTriggerUnit();
                if (!building.IsTaxable()) return;
                TryRemove(building);
            }, "registering building"));
        }

        public static void AddTax(unit shop, int pretax)
        {
            ShopInfo info = shop.GetTypeID();
            if (info == null) return;
            ChangeGold(shop, MathRound(pretax * info.TaxRate));
        }

        public static bool IsTaxable(this unit building)
        {
            return building.IsStructure() && (building.IsGuild() || building.IsShop());
        }

        public static Building Get(unit unit)
        {
            if (buildingMap.TryGetValue(unit, out var building)) return building;
            return null;
        }

        public static bool TryRegister(unit building)
        {
            if (!building.IsTaxable()) return false;
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
