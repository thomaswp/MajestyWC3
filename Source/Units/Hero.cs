using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static War3Api.Common;
using static War3Api.Blizzard;
using Source;
using Source.Behaviors;

namespace Source.Units
{
    public abstract class Hero : UnitAI
    {

        public override void OnAttack(unit target)
        {
            base.OnAttack(target);

            int exp = GetUnitLevel(target);
            AddHeroXP(Unit, exp, true);
        }

        public override void OnAttacked(unit attacker)
        {
            base.OnAttacked(attacker);
        }

        public override void OnHomeAttacked(unit attacker)
        {
            base.OnHomeAttacked(attacker);
            Console.WriteLine("Defending...");
            foreach (DefendHome b in behaviors.Where(b => b is DefendHome))
            {
                behavior = b;
                b.OnHomeAttacked(attacker);
                break;
            }
        }

    }
}
