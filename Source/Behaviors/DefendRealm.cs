﻿using Source.Units;
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
    public class DefendRealm : DefendBuilding
    {
        public override string GetStatusGerund()
        {
            return $"defending the realm against {Target.GetName()}";
        }
    }
}
