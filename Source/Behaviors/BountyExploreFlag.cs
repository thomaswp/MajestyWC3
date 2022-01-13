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
        const float DISTANCE_TO_MOTIVATION_FACTOR = 0.001f;

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

        public override bool TryInterrupt(Behavior with)
        {
            if (with is Flee && IsTargetStillValid(Target))
            {
                float motivation = AI.GetBountyMotivation(targetFlag);
                float confidence = AI.GetFightConfidence(AI.Unit.GetLocation());
                //Console.WriteLine($"Trying to interrupt with confidence {confidence} * {motivation}");
                if (confidence * motivation > 1) return false;
                //Console.WriteLine("Success");
            }
            return base.TryInterrupt(with);
        }

        public override bool Update()
        {
            //Console.WriteLine($"Updating exploration...");
            if (!base.Update()) return false;
            if (AI.Unit.DistanceTo(targetFlag) < 100)
            {
                int bounty = targetFlag.GetFlagBounty();
                AI.ReceiveBounty(bounty);
                KillUnit(targetFlag);
                return false;
            }
            return true;
        }

        protected override bool IsTargetStillValid(location target)
        {
            // No base call, since we don't care about if it's explored
            return targetFlag != null && !targetFlag.IsDead();
        }

        private float GetBountyDesire(unit flag)
        {
            if (flag == null) return 0;
            return AI.GetBountyMotivation(targetFlag) / (AI.Unit.DistanceTo(flag) * DISTANCE_TO_MOTIVATION_FACTOR);
        }

        protected override location SelectTarget()
        {
            var flags = GetUnitsOfTypeIdAll(Constants.UNIT_EXPLORE_FLAG).ToList();
            if (flags.Count == 0) return null;
            targetFlag = flags
                .OrderBy(u => GetBountyDesire(u))
                .First();
            return targetFlag.GetLocation();
        }

        protected override void OnTargetCleared()
        {
            base.OnTargetCleared();
            targetFlag = null;
        }
    }
}
