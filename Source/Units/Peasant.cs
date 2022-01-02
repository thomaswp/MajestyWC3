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
        protected override void AddBehaviors()
        {
            AddBehavior(new Behaviors.Build(), 1);
        }
    }
}
