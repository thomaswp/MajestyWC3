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

        public abstract bool CanStart();
        public abstract void Start();
        public abstract void Stop();
        public abstract bool Update();

        public virtual string GetName()
        {
            return GetType().Name;
        }

        public void init(UnitAI ai, int weight)
        {
            AI = ai;
            Weight = weight;
        }
    }
}
