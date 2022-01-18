using Source.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source.Behaviors
{

    public abstract class Behavior
    {
        public UnitAI AI { get; private set; }
        public int Weight { get; private set; }
        public bool NeedsRestart { get; set; }

        public abstract bool CanStart();
        public abstract void Start();
        public abstract void Stop();
        public abstract bool Update();
        public abstract string GetStatusGerund();

        public virtual float StartWeight()
        {
            return CanStart() ? 1 : 0;
        }

        public virtual bool TryInterrupt(Behavior with)
        {
            Stop();
            return true;
        }

        public virtual string GetName()
        {
            return GetType().Name;
        }

        public override string ToString()
        {
            return GetName();
        }

        public virtual bool IsInCombatOrDanger()
        {
            return false;
        }

        public void init(UnitAI ai, int weight)
        {
            AI = ai;
            Weight = weight;
        }
    }
}
