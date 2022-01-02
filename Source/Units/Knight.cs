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
    public class Knight : Hero
    {
        protected override void AddBehaviors()
        {
            AddBehavior(new Behaviors.Explore(), 3);
            AddBehavior(new Behaviors.DefendHome());
            AddBehavior(new Behaviors.Fight(), 10);
            AddBehavior(new Behaviors.Shop(), 5);
            //AddBehavior(Behaviors.Activity.Fleeing);
        }
    }
}
