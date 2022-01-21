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
    public class DefendBuilding : Fight
    {
        public const int MIN_HOME_ATTACK_DIS = 1000;

        protected unit buildingDefended;

        public override string GetStatusGerund()
        {
            return $"defending their home against {Target.GetName()}";
        }

        protected override bool IsTargetCloseEnough(unit target)
        {
            return target.DistanceTo(buildingDefended) <= MIN_HOME_ATTACK_DIS;
        }

        public override bool TryInterrupt(Behavior with)
        {
            //Console.WriteLine("No interrupt for you!");
            // TODO: More nuance
            return false;
        }

        public override void Start()
        {
            base.Start();
            //Console.WriteLine($"{AI.Unit.GetName()} defending home from {Target.GetName()}");
        }

        public void OnHomeAttackedBy(unit attacker, unit toDefend)
        {
            Target = attacker;
            buildingDefended = toDefend;
        }

        protected override unit SelectTarget()
        {
            // Target can only be selected by home attack
            return null;
        }
    }
}
