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
    public class Build : Behavior
    {

        protected unit buildingTarget;

        public override bool CanStart()
        {
            SelectTarget();
            return buildingTarget != null;
        }

        public override void Start()
        {
            SelectTarget();
            if (buildingTarget == null) return;
            //Console.WriteLine($"Building {GetUnitName(buildingTarget)}");
            IssueTargetOrderById(AI.unit, Constants.ORDER_REPAIR, buildingTarget);
        }

        public override void Stop()
        {
            buildingTarget = null;
        }

        public override bool Update()
        {
            return TargetNeedsRepair();
        }

        private bool TargetNeedsRepair()
        {
            return buildingTarget != null && GetUnitLifePercent(buildingTarget) < 99.95;
        }

        private void SelectTarget()
        {
            if (TargetNeedsRepair()) return;

            buildingTarget = null;

            //Console.WriteLine("Start search...");
            var units = GetUnitsOfPlayerAll(AI.humanPlayer).ToList();
            //Console.WriteLine($"Searching {units.Count} units...");
            var repairable = units
                .Where(unit => IsUnitIdType(GetUnitTypeId(unit), UNIT_TYPE_STRUCTURE))
                .Where(unit => GetUnitLifePercent(unit) < 99.95);
            //Console.WriteLine($"Found {repairable.Count()} repairable...");
            if (repairable.Count() == 0) return;
            buildingTarget = repairable
                .OrderBy(other => AI.unit.DistanceTo(other))
                .First();
        }
    }
}
