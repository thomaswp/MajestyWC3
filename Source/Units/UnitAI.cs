using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static War3Api.Common;
using static War3Api.Blizzard;
using Source;
using WCSharp.Events;
using Source.Behaviors;
using Source.Items;

namespace Source.Units
{
    public abstract class UnitAI
    {

        public unit Unit { get; private set; }
        public player MyPlayer { get; private set; }
        public player HumanPlayer { get; private set; }

        public unit Home { get; private set; }
        public unit InBuilding { get; set; }

        public int gold { get; set; }

        protected List<Behavior> behaviors = new List<Behavior>();
        protected Behavior behavior = null;

        public UnitAI()
        {
        }

        protected abstract void AddBehaviors();

        protected void AddBehavior(Behavior behavior, int weight = 0)
        {
            behavior.init(this, weight);
            behaviors.Add(behavior);
            Console.WriteLine($"Adding {behavior.GetName()} for {Unit.GetName()}");
        }

        private void init(unit unit)
        {
            this.Unit = unit;
            MyPlayer = GetOwningPlayer(unit);
            HumanPlayer = MyPlayer.GetHumanForAI();
            AddBehaviors();
        }

        public virtual void Update()
        {
            if (behaviors == null) return;
            //Console.WriteLine("Starting update...");
            if (behavior == null)
            {
                behavior = ChooseIdleBehavior();
                if (behavior != null)
                {
                    //Console.WriteLine($"Starting {behavior.GetActivity()} for {unit.GetName()}");
                    //Console.WriteLine("Starting!");
                    behavior.Start();
                }
            }

            if (behavior == null) return;

            //Console.WriteLine("Updating behavior...");
            if (!behavior.Update())
            {
                behavior.Stop();
                behavior = null;
            }
        }

        protected virtual Behavior ChooseIdleBehavior()
        {
            //Console.WriteLine($"Selecting from {behaviors.Count} behaviors");
            var possible = behaviors.Where(b => b.Weight > 0 && b.CanStart());
            int den = possible.Select(b => b.Weight).Sum();
            if (den == 0) return null;
            float rand = GetRandomInt(0, 999) / 1000f;
            foreach (Behavior b in possible)
            {
                float slice = (float) b.Weight / den;
                if (rand <= slice) return b;
                rand -= slice;
            }
            Console.WriteLine("no behaviors...");
            return null;
        }

        static Dictionary<unit, UnitAI> unitMap = new Dictionary<unit, UnitAI>();
        static Dictionary<unit, List<UnitAI>> homeMap = new Dictionary<unit, List<UnitAI>>();

        public static UnitAI RegisterUnit(unit unit)
        {
            if (unit.IsStructure()) return null;

            if (!unitMap.ContainsKey(unit))
            {
                int unitID = GetUnitTypeId(unit);
                UnitAI ai = null;
                switch (unitID)
                {
                    case Constants.UNIT_PEASANT_WORKER:
                        ai = new Peasant();
                        break;
                    case Constants.UNIT_KNIGHT:
                        ai = new Knight();
                        break;
                    case Constants.UNIT_BUILDER:
                        return null;
                }
                if (ai != null)
                {
                    Console.WriteLine($"Creating new AI for {unit.Name()}");
                    ai.init(unit);
                    unitMap[unit] = ai;
                }
                else
                {
                    Console.WriteLine($"Unknown AI for {unit.Name()}");
                }
            }
            return unitMap[unit];
        }

        public void SetHome(unit home)
        {
            if (this.Home != null)
            {
                homeMap[this.Home].Remove(this);
            }
            this.Home = home;
            if (!homeMap.ContainsKey(home))
            {
                homeMap[home] = new List<UnitAI>();
            }
            var units = homeMap[home];
            units.Add(this);
            OnHomeSet();
        }
        protected virtual IEnumerable<int> GetWantedItemsList()
        {
            return new List<int>();
        }

        public IEnumerable<int> GetWantedItems()
        {
            // TODO: Should probably cache
            Dictionary<int, int> itemCounts = new Dictionary<int, int>();
            foreach (int item in GetWantedItemsList())
            {
                if (!itemCounts.TryGetValue(item, out int count))
                {
                    count = itemCounts[item] = Unit.GetItemCount(item);
                }
                if (count > 0)
                {
                    itemCounts[item]--;
                }
                else
                {
                    yield return item;
                }
            }
        }

        public bool TryPurchase(int itemID)
        {
            int cost = Items.Items.GetItemCost(itemID);
            if (gold < cost) return false;
            item item;
            bool bought;
            if (Unit.HasItem(itemID))
            {
                item = GetItemOfTypeFromUnitBJ(Unit, itemID);
                SetItemCharges(item, GetItemCharges(item) + 1);
                bought = true;
            }
            else
            {
                item = CreateItem(itemID, 0, 0);
                bought = UnitAddItem(Unit, item);
            }
            if (bought) gold -= cost;
            return bought;
        }

        public virtual void OnAttack(unit target)
        {

        }

        public virtual void OnAttacked(unit attacker)
        {

        }

        public virtual void OnHomeAttacked(unit attacker)
        {

        }

        public virtual void OnHomeSet()
        {

        }

        public static void Trigger(PlayerUnitEvent ev, unit unit)
        {
            if (!unitMap.ContainsKey(unit)) return;
            UnitAI ai = unitMap[unit];
            try
            {
                switch (ev)
                {
                    case PlayerUnitEvent.UnitTypeAttacks:
                        ai.OnAttack(GetAttackedUnitBJ());
                        break;
                    case PlayerUnitEvent.UnitTypeIsAttacked:
                        ai.OnAttacked(GetAttacker());
                        break;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error with {GetUnitName(ai.Unit)} event {ev}: {e.Message}");
            }
        }

        public static void TriggerHomeAttacked(unit home, unit attacker)
        {
            try
            {
                if (!home.IsStructure() || !homeMap.ContainsKey(home)) return;
                //Console.WriteLine($"Checking home {home.GetName()} attacked by {attacker.GetName()}...");
                var units = homeMap[home];
                foreach (UnitAI unit in units) unit.OnHomeAttacked(attacker);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error handing home {home.GetName()} attacked by {attacker.GetName()}: {e.Message}");
            }
        }

        public static void UpdateUnits()
        {
            //Console.WriteLine($"Updating {unitMap.Count} units...");
            foreach (var unit in unitMap.Values)
            {
                try
                {
                    unit.Update();
                } catch (Exception e)
                {
                    Console.WriteLine($"Error updating {GetUnitName(unit.Unit)}: {e.Message}");
                }
            }
        }
    }
}
