using Source.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source.Units.Monsters
{
    public abstract class Monster : FighterAI
    {
        protected override void AddBehaviors()
        {
            AddBehavior(new Wander(), 5);
            AddBehavior(new Fight(), 10);
            AddBehavior(new DefendHome());
        }
    }
}
