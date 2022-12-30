using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static War3Api.Common;
using static War3Api.Blizzard;

namespace Source.MapEvents
{
    public delegate void GPLAction();

    public class Agent
    {
        public dynamic this[string key]
        {
            get => null;
            set { }
        }

        public unit ToUnit()
        {
            // TODO
            return null;
        }
    }

    public class List
    {

    }
}
