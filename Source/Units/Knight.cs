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
            AddBehavior(Behaviors.Activity.Exploring, 3);
            AddBehavior(Behaviors.Activity.DefendingHome);
            AddBehavior(Behaviors.Activity.Fighting, 10);
            //AddBehavior(Behaviors.Activity.Fleeing);
        }
    }
}
