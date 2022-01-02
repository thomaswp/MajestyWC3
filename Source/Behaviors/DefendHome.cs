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
        public const int MIN_HOME_ATTACK_DIS = 800;

        public override bool Update()
        {
            return !IsTargetValid();
        }

        private bool IsTargetValid()
        {
            return targetEnemy != null && !targetEnemy.IsDead() && 
                targetEnemy.DistanceTo(AI.home) <= MIN_HOME_ATTACK_DIS;
        }

        public void OnHomeAttacked(unit attacker)
        {
            if (IsTargetValid()) return;
            targetEnemy = attacker;
            Console.WriteLine($"{AI.unit.GetName()} defending home from {targetEnemy.GetName()}");
            IssueTargetOrder(AI.unit, "Attack", targetEnemy);
        }

        protected override void SelectTarget() { }
    }
}
