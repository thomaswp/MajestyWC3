using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static War3Api.Common;
using static War3Api.Blizzard;
using System.Diagnostics.CodeAnalysis;

namespace Source.MapEvents
{
    public delegate void GPLAction();

    public class Agent
    {
        // Can be null
        public readonly unit Unit;

        public Agent(unit unit)
        {
            this.Unit = unit;
        }

        public dynamic this[string key]
        {
            get => null;
            set { }
        }

        public static implicit operator unit(Agent agent)
        {
            return agent.Unit;
        }

        public string Title
        {
            get
            {
                // TODO
                return "";
            }
        }
    }

    public class List
    {
        public List() { }

        public List(List<Agent> items)
        {
            this.items = items;
        }

        public readonly List<Agent> items = new List<Agent>();
    }
}
