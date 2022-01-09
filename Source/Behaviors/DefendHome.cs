using Source.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static War3Api.Common;
using static War3Api.Blizzard;
using Source;

namespace Source.Behaviors
{
    public class DefendHome : Fight
    {
        public const int MIN_HOME_ATTACK_DIS = 1000;

        protected override bool IsValidTarget()
        {
            return targetEnemy != null && !targetEnemy.IsDead() && 
                targetEnemy.DistanceTo(AI.Home) <= MIN_HOME_ATTACK_DIS;
        }

        public override bool TryInterrupt(Behavior with)
        {
            //Console.WriteLine("No interrupt for you!");
            // TODO: More nuance
            return false;
        }

        public override void Start()
        {
            base.Start();
            Console.WriteLine($"{AI.Unit.GetName()} defending home from {targetEnemy.GetName()}");
        }

        public override void Stop()
        {
            base.Stop();
            Console.WriteLine("Stopping Defend home");
        }

        public void OnHomeAttackedBy(unit attacker)
        {
            targetEnemy = attacker;
        }

        protected override void SelectTarget() { }
    }
}
