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
    public class Build : TargetBehavior<unit>
    {
        public override string GetStatusGerund()
        {
            return $"building a {Target.GetName()}";
        }

        public override void Start()
        {
            base.Start();
            IssueTargetOrderById(AI.Unit, Constants.ORDER_REPAIR, Target);
        }

        protected override bool IsTargetStillValid(unit target)
        {
            return base.IsTargetStillValid(target) && !target.IsDead() && GetUnitLifePercent(target) < 99.95;
        }
        
        protected override unit SelectTarget()
        {
            //Console.WriteLine("Start search...");
            var units = GetUnitsOfPlayerAll(AI.HumanPlayer).ToList();
            //Console.WriteLine($"Searching {units.Count} units...");
            var repairable = units
                .Where(unit => IsUnitIdType(GetUnitTypeId(unit), UNIT_TYPE_STRUCTURE))
                .Where(unit => GetUnitLifePercent(unit) < 99.95);
            //Console.WriteLine($"Found {repairable.Count()} repairable...");
            if (!repairable.Any()) return null;
            return repairable
                .OrderBy(other => AI.Unit.DistanceTo(other))
                .First();
        }
    }
}
