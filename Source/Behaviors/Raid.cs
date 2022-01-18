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
    public class Raid : Fight
    {
        const int MAX_RAID_RANGE = 5000;

        public override string GetStatusGerund()
        {
            //Console.WriteLine(AI.Unit.GetName() + " raiding " + Target.GetName());
            return $"raiding a {Target.GetName()}";
        }

        protected override bool IsTargetCloseEnough(unit target)
        {
            return true;
        }

        protected override void CheckForNewTarget()
        {
            // Don't swap to non-buildings, just allow updating
        }

        //protected override bool IsTargetStillValid(unit target)
        //{
        //    // TODO: Shouldn't always interrupt raids for nearby enemies...
        //    // maybe this should just be a broader system where allies can ask for help
        //    // regardless of whether you're exploring, etc.
        //    return base.IsTargetStillValid(target) &&
        //        // If there's a valid non-structure to target, end raiding
        //        !GetPossibleTargets().Where(t => !t.IsStructure()).Any();
        //}

        protected override unit SelectTarget()
        {
            bool checkVisible = !AI.IsNeutralHostile;
            player player = AI.HumanPlayer;
            var visible = GetUnitsInRangeOfLocAll(MAX_RAID_RANGE, AI.Unit.GetLocation()).ToList()
                .Where(u => !checkVisible || !IsLocationMaskedToPlayer(u.GetLocation(), player))
                .Where(u => AI.IsEnemy(u) && !u.IsDead() && u.IsStructure());
            if (!visible.Any()) return null;
            // TODO: Include other factors (e.g. intimidation)
            return visible.OrderBy(u => u.DistanceTo(AI.Unit)).First();
        }
    }
}
