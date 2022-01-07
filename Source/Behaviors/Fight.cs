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
    public class Fight : Behavior
    {

        protected unit targetEnemy;

        public override bool CanStart()
        {
            SelectTarget();
            return targetEnemy != null;
        }

        public override void Start()
        {
            SelectTarget();
            if (targetEnemy == null) return;
            IssueTargetOrder(AI.Unit, "Attack", targetEnemy);
            Console.WriteLine(AI.Unit.GetName() + " targeting " + targetEnemy.GetName());
        }

        public override void Stop()
        {
            targetEnemy = null;
        }

        public override bool Update()
        {
            if (!IsValidTarget()) return false;

            // TODO: Check for danger...
            //int totalEnemyHP = visible.Where(u => !u.IsStructure()).Select(u => u.GetHP()).Sum();

            return true;
        }

        private bool IsValidTarget()
        {
            return targetEnemy != null && !targetEnemy.IsDead();
        }

        protected virtual void SelectTarget()
        {
            if (IsValidTarget() && targetEnemy.DistanceTo(AI.Unit) < AI.Unit.GetSightRange()) return;

            targetEnemy = null;

            player neutral = Player(PLAYER_NEUTRAL_AGGRESSIVE);
            var visible = GetUnitsInRangeOfLocAll(AI.Unit.GetSightRange(), AI.Unit.GetLocation()).ToList()
                .Where(u => GetOwningPlayer(u) == neutral)
                .Where(u => !u.IsDead());

            if (visible.Count() == 0) return;

            targetEnemy = visible.OrderBy(u => (u.IsStructure() ? 1000 : 0) + u.DistanceTo(AI.Unit)).First();
        }
    }
}
