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
        public string Name { get { return Unit.GetName(); } }
        public player MyPlayer { get; private set; }
        public player HumanPlayer { get; private set; }
        public player AIPlayer { get; private set; }

        public unit Home { get; private set; }
        public unit InBuilding { get; set; }
        public bool IsInBuilding { get { return InBuilding != null; } }
        public bool IsHome { get { return InBuilding != null && InBuilding == Home; } }

        public int GoldTaxed { get; set; }
        public int GoldUntaxed { get; set; }

        const float RECOVER_RATE = 0.01f, RECOVER_RATE_AT_HOME = RECOVER_RATE * 2;

        public int Gold 
        { 
            get { return GoldTaxed + GoldUntaxed; } 
            set
            {
                int oldGold = Gold;
                // Gaining gold, goes to untaxed by default
                if (value > oldGold) GoldUntaxed += value - oldGold;
                else
                {
                    // Losing gold, comes from taxed first, then untaxed
                    int loss = oldGold - value;
                    if (loss > GoldTaxed) GoldUntaxed -= loss - GoldTaxed;
                    GoldTaxed = Math.Max(0, GoldTaxed - loss);
                }
            }
        }

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
            //Console.WriteLine($"Adding {behavior.GetName()} for {Unit.GetName()}");
        }

        public Behavior GetBehavior(Type type)
        {
            return behaviors.Find(b => type.IsInstanceOfType(b)) ?? null;
        }

        public bool TryInterruptWith(Type type, bool checkIfCanStart)
        {
            Behavior b = GetBehavior(type);
            if (b == null) return false;
            return TryInterruptWith(b, checkIfCanStart);
        }

        public bool TryInterruptWith(Behavior newBehavior, bool checkIfCanStart)
        {
            if (behavior == newBehavior) return false;
            if (checkIfCanStart && !newBehavior.CanStart()) return false;
            if (behavior != null && !behavior.TryInterrupt(newBehavior)) return false;
            Console.WriteLine($"{Name} interrupted with {newBehavior.GetName()}");
            behavior = newBehavior;
            newBehavior.Start();
            return true;
        }

        protected virtual void Init(unit unit)
        {
            Unit = unit;
            MyPlayer = GetOwningPlayer(unit);
            AIPlayer = MyPlayer.GetAIForHuman();
            HumanPlayer = MyPlayer.GetHumanForAI();
            AddBehaviors();
        }

        protected virtual void RegainLife()
        {
            if (InBuilding == null) return;
            float rate = IsHome ? RECOVER_RATE_AT_HOME : RECOVER_RATE;
            SetUnitLifeBJ(Unit, Unit.GetHP() * (1 + rate));
        }

        public void EnterBuilding(unit building)
        {
            InBuilding = building;
            ShowUnitHide(Unit);
            PauseUnit(Unit, true);
        }

        public void ExitBuilding()
        {
            // TODO: move to bottom of building?
            InBuilding = null;
            ShowUnitShow(Unit);
            PauseUnit(Unit, false);
        }

        public bool IsEnemy(unit unit)
        {
            // TODO: Alliances
            player owner = unit.GetPlayer();
            //Console.WriteLine($"{unit.GetName()} owned by {GetPlayerId(owner)} vs " +
            //    $"H:{GetPlayerId(HumanPlayer)} and AI:{GetPlayerId(AIPlayer)}");
            return owner != HumanPlayer && owner != AIPlayer;
        }

        public virtual float GetIntimidation(unit enemy)
        {
            // TODO: Consistent system
            return GetUnitLevel(enemy) * enemy.GetHP();
        }

        public virtual float GetConfidence(unit ally)
        {
            // TODO: Consistent system
            return  (1 + (GetUnitLevel(ally) - 1) / 0.2f) * ally.GetHP();
        }

        public virtual void Update()
        {
            RegainLife();
            DoPreBehaviorActions();
            if (behaviors == null) return;
            //Console.WriteLine("Starting update...");
            if (behavior == null)
            {
                behavior = ChooseIdleBehavior();
                if (behavior != null)
                {
                    Console.WriteLine($"Starting {behavior.GetName()} for {Unit.GetName()}");
                    behavior.Start();
                }
            }

            if (behavior == null) return;

            //Console.WriteLine("Updating behavior...");
            if (behavior.NeedsRestart)
            {
                Console.WriteLine($"Restarting {behavior}");
                behavior.NeedsRestart = false;
                behavior.Start();
            }
            if (!behavior.Update())
            {
                behavior.Stop();
                behavior = null;
            }
        }

        protected virtual void DoPreBehaviorActions()
        {
            
        }

        protected virtual Behavior ChooseIdleBehavior()
        {
            //Console.WriteLine($"Selecting from {behaviors.Count} behaviors");
            var weights = behaviors.ToDictionary(b => b, b => b.StartWeight() * b.Weight);
            var possible = behaviors.Where(b => weights[b] > 0);
            float den = possible.Select(b => weights[b]).Sum();
            if (den == 0) return null;
            float rand = GetRandomInt(0, 999) / 1000f;
            foreach (Behavior b in possible)
            {
                float slice = weights[b] / den;
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
                    Console.WriteLine($"Creating new AI for {unit.GetName()}");
                    ai.Init(unit);
                    unitMap[unit] = ai;
                }
                else
                {
                    Console.WriteLine($"Unknown AI for {unit.GetName()}");
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
            if (Gold < cost) return false;
            item item;
            bool bought = false;
            if (Unit.HasItem(itemID))
            {
                item = GetItemOfTypeFromUnitBJ(Unit, itemID);

                SetItemCharges(item, GetItemCharges(item) + 1);
                bought = true;
            }
            else if (UnitInventoryCount(Unit) < 6)
            {
                UnitAddItemById(Unit, itemID);
                bought = true;
            }
            if (bought) Gold -= cost;
            Console.WriteLine($"{Unit.GetName()} bought {itemID} successful: {bought}");

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
