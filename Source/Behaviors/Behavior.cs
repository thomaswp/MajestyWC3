using Source.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source.Behaviors
{
    public enum Activity
    {
        Idle,
        Exploring,
        Fighting,
        Fleeing,
        DefendingHome,
        Building,
    }

    public abstract class Behavior
    {
        public UnitAI AI { get; private set; }
        public int Weight { get; private set; }

        public abstract Activity GetActivity();

        public abstract bool CanStart();
        public abstract void Start();
        public abstract void Stop();
        public abstract bool Update();

        protected Behavior() { }

        public static Behavior FromActivity(Activity activity, UnitAI ai, int weight)
        {
            Behavior b = null;
            switch(activity)
            {
                case Activity.Building: b = new Build(); break;
                case Activity.DefendingHome: b = new DefendHome(); break;
                case Activity.Exploring: b = new Explore(); break;
                case Activity.Fighting: b = new Fight(); break;
            }
            if (b != null)
            {
                b.AI = ai;
                b.Weight = weight;
            }
            return b;
        }
    }
}
