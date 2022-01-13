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
    public class BountyAttackFlag : Fight
    {

        protected unit targetFlag;

        public override float StartWeight()
        {
            if (!CanStart()) return 0;
            return GetBountyDesire(targetFlag);
        }

        public override void Start()
        {
            base.Start();
            Console.WriteLine(AI.Unit.GetName() + " pursuing bounty");
        }

        public override bool TryInterrupt(Behavior with)
        {
            if (with is Flee && IsTargetStillValid(Target))
            {
                //Console.WriteLine("Trying to interrupt");
                if (GetBountyDesire(targetFlag) > 1) return false;
                //Console.WriteLine("Success");
            }
            return base.TryInterrupt(with);
        }

        protected override bool IsTargetStillValid(unit target)
        {
            return base.IsTargetStillValid(target) && targetFlag != null
                && !targetFlag.IsDead();
        }

        protected override bool IsTargetCloseEnough(unit target)
        {
            return true;
        }

        protected virtual float GetBountyDesire(unit flag)
        {
            float confidence = AI.GetFightConfidence(Bounties.GetFlagTarget(flag));
            confidence *= AI.GetBountyMotivation(flag);
            //Console.WriteLine("Confidence: " + confidence);
            return confidence;
        }

        protected override unit SelectTarget()
        {
            var flags = GetUnitsOfTypeIdAll(Constants.UNIT_ATTACK_FLAG).ToList()
                .Where(u => !IsLocationMaskedToPlayer(u.GetLocation(), AI.HumanPlayer))
                .ToList();
            if (flags.Count == 0) return null;
            targetFlag = flags
                .OrderBy(u => GetBountyDesire(u))
                .First();
            unit targetUnit = Bounties.GetFlagTarget(targetFlag);
            //Console.WriteLine($"Bounty selecting {targetUnit.GetName()} with {GetBountyDesire(targetFlag)} desire");
            return targetUnit;
        }
    }
}
