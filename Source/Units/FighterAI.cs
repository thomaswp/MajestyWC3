using Source.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static War3Api.Common;

namespace Source.Units
{
    public abstract class FighterAI : UnitAI
    {

        public override void OnAttacked(unit attacker)
        {
            base.OnAttacked(attacker);

            //Console.WriteLine("Trying Flee");
            if (TryInterruptWith(typeof(Flee), true)) return;
            //Console.WriteLine("Trying fight");
            if (TryInterruptWith(typeof(Fight), true))
            {
                // TODO: call allies to help you
                return;
            }
            //if (!(behavior is Fight)) Console.WriteLine("Failed...");

        }

        public override void OnHomeAttacked(unit attacker)
        {
            base.OnHomeAttacked(attacker);
            DefendBuilding d = (DefendBuilding)GetBehavior(typeof(DefendBuilding));
            if (d == null) return;
            d.OnHomeAttackedBy(attacker, Home);
            TryInterruptWith(d, false);
        }
    }
}
