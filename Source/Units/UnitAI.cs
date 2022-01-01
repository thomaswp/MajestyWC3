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

namespace Source.Units
{
    public abstract class UnitAI
    {

        public unit unit { get; private set; }
        public player myPlayer { get; private set; }
        public player humanPlayer { get; private set; }

        public unit home { get; private set; }

        protected List<Behavior> behaviors = new List<Behavior>();
        protected Behavior behavior = null;

        public UnitAI()
        {
        }

        protected abstract void AddBehaviors();

        protected void AddBehavior(Activity activity, int weight = 0)
        {
            Behavior b = Behavior.FromActivity(activity, this, weight);
            if (b == null)
            {
                Console.WriteLine($"no behavior for {activity}");
                return;
            }
            behaviors.Add(b);
            Console.WriteLine($"Adding {activity} for {unit.GetName()}");
        }

        private void init(unit unit)
        {
            this.unit = unit;
            myPlayer = GetOwningPlayer(unit);
            humanPlayer = myPlayer.GetHumanForAI();
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
            if (this.home != null)
            {
                homeMap[this.home].Remove(this);
            }
            this.home = home;
            if (!homeMap.ContainsKey(home))
            {
                homeMap[home] = new List<UnitAI>();
            }
            var units = homeMap[home];
            units.Add(this);
            OnHomeSet();
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
                Console.WriteLine($"Error with {GetUnitName(ai.unit)} event {ev}: {e.Message}");
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
                    Console.WriteLine($"Error updating {GetUnitName(unit.unit)}: {e.Message}");
                }
            }
        }
    }
}
