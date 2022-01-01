using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static War3Api.Common;
using static War3Api.Blizzard;
using Source;

namespace Source.Units
{
    public class Peasant : UnitAI
    {
        unit buildingTarget;

        public override void Update()
        {
            //Console.WriteLine("Updating Peasant");
            UpdateBuildingTarget();   
            //GetUnitsOfPlayerMatching()
        }

        private void UpdateBuildingTarget()
        {
            if (buildingTarget != null)
            {
                if (GetUnitLifePercent(buildingTarget) < 99.95)
                {
                    return;
                }
            }

            //Console.WriteLine("Start search...");
            var units = GetUnitsOfPlayerAll(humanPlayer).ToList();
            //Console.WriteLine($"Searching {units.Count} units...");
            var repairable = units
                .Where(unit => IsUnitIdType(GetUnitTypeId(unit), UNIT_TYPE_STRUCTURE))
                .Where(unit => GetUnitLifePercent(unit) < 99.95);
            if (repairable.Count() == 0) return;
            buildingTarget = repairable
                .OrderBy(other => unit.DistanceTo(other))
                .First();
            //Console.WriteLine("End search...");

            if (buildingTarget == null) return;

            Console.WriteLine($"Building {GetUnitName(buildingTarget)}");
            IssueTargetOrderById(unit, Constants.ORDER_REPAIR, buildingTarget);
            //IssueTargetOrder(unit, "Attack", buildingTarget);
            //IssueTargetOrder(unit, "Human Peasant - Repair", buildingTarget);
        }
    }
}
