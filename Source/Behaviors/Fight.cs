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
    public class Fight : TargetBehavior<unit>
    {
        public override string GetStatusGerund()
        {
            //Console.WriteLine(AI.Unit.GetName() + " targeting " + Target.GetName());
            return $"fighting a {Target.GetName()}";
        }

        public override void Start()
        {
            base.Start();
            IssueTargetOrder(AI.Unit, "Attack", Target);
        }

        public override bool IsInCombatOrDanger()
        {
            return true;
        }

        protected override int GetTargetTimeout()
        {
            return 10;
        }
        public override bool Update()
        {
            CheckForNewTarget();
            return base.Update();
        }

        protected virtual void CheckForNewTarget()
        {
            // If targeting a structure but there's a better target...
            if (Target.IsStructure() && RefreshTarget())
            {
                //Console.WriteLine($"{AI.Name} retargeting {Target.GetName()}");
                // Refresh order
                Start();
            }
        }

        protected override bool IsTargetStillValid(unit target)
        {
            return base.IsTargetStillValid(target) && !target.IsDead() &&
                IsTargetCloseEnough(target);
        }

        protected virtual bool IsTargetCloseEnough(unit target)
        {
            return target.DistanceTo(AI.Unit) < AI.Unit.GetSightRange();
        }

        protected IEnumerable<unit> GetPossibleTargets()
        {
            return GetUnitsInRangeOfLocAll(AI.Unit.GetSightRange(), AI.Unit.GetLocation()).ToList()
                .Where(u => AI.IsEnemy(u) && !u.IsDead()); 
        }

        protected override unit SelectTarget()
        {
            var visible = GetPossibleTargets();
            if (!visible.Any()) return null;
            return visible.OrderBy(u => (u.IsStructure() ? 1000 : 0) + u.DistanceTo(AI.Unit)).First();
        }
    }
}
