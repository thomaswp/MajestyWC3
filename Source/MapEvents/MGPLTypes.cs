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

    public class Agent
    {
        // Can be null
        public readonly unit Unit;
        private Dictionary<string, AgentProperty> properties = new Dictionary<string, AgentProperty>();

        public Agent(unit unit)
        {
            this.Unit = unit;
        }

        public AgentProperty this[string key]
        {
            get => properties[key];
            set { properties[key] = value; }
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
