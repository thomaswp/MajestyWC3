using Source.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static War3Api.Common;
using static War3Api.Blizzard;
using Source;
using Source.Interface;

namespace Source.Behaviors
{
    public class BountyExploreFlag : Explore
    {
        protected unit targetFlag;

        // TODO: Per Hero
        const float DISTANCE_TO_BOUNTY_FACTOR = 0.1f;

        public override bool CanStart()
        {
            SelectTarget();
            return exploringLocation != null;
        }

        public override float StartWeight()
        {
            if (!CanStart()) return 0;
            return GetBountyDesire(targetFlag);
        }

        public override void Start()
        {
            base.Start();
            Console.WriteLine($"Knight pursuing bounty explore flag");
        }

        public override void Stop()
        {
            base.Stop();
            targetFlag = null;
        }

        public override bool Update()
        {
            //Console.WriteLine($"Updating exploration...");
            if (!base.Update()) return false;
            if (AI.Unit.DistanceTo(targetFlag) < 100)
            {
                AI.GoldUntaxed += targetFlag.GetFlagBounty();
                KillUnit(targetFlag);
                return false;
            }
            return IsValidTarget();
        }

        private bool IsValidTarget()
        {
            return targetFlag != null && !targetFlag.IsDead();
        }

        private float GetBountyDesire(unit flag)
        {
            if (flag == null) return 0;
            return flag.GetFlagBounty() / (AI.Unit.DistanceTo(flag) * DISTANCE_TO_BOUNTY_FACTOR);
        }

        protected override void SelectTarget()
        {
            if (IsValidTarget()) return;

            var flags = GetUnitsOfTypeIdAll(Constants.UNIT_EXPLORE_FLAG).ToList();
            if (flags.Count == 0) return;
            targetFlag = flags
                .OrderBy(u => GetBountyDesire(u))
                .First();
            exploringLocation = targetFlag.GetLocation();
        }
    }
}
