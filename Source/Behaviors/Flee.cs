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
    public class Flee : RestAtHome
    {
        Random rand = new Random();

        public override bool CanStart()
        {
            return IsInDanger((float)rand.NextDouble());
        }

        public override void Start()
        {
            base.Start();
            Console.WriteLine($"{AI.Name} fleeing enemiez");
        }
        public override bool IsInCombatOrDanger()
        {
            return true;
        }

        public bool IsInDanger(float fearFactor)
        {
            if (fearFactor == 0) return false;
            float confidence = AI.GetFightConfidence(AI.Unit.GetLocation());
            return confidence > fearFactor;
        }
    }
}
