using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source.MapEvents
{
    public class AgentProperty
    {
        private object _value;

        // Constructor to wrap any object
        public AgentProperty(object value)
        {
            _value = value;
        }

        // Implicit conversion from AgentProperty to primitive types
        public static implicit operator int(AgentProperty wrapper) => (int)wrapper._value;
        public static implicit operator double(AgentProperty wrapper) => (double)(wrapper._value);
        public static implicit operator bool(AgentProperty wrapper) => (bool)(wrapper._value);
        public static implicit operator string(AgentProperty wrapper) => (string)(wrapper._value);
        public static implicit operator Action(AgentProperty wrapper) => (Action)wrapper._value;

        // Implicit conversion from primitive types to AgentProperty
        public static implicit operator AgentProperty(int value) => new AgentProperty(value);
        public static implicit operator AgentProperty(double value) => new AgentProperty(value);
        public static implicit operator AgentProperty(bool value) => new AgentProperty(value);
        public static implicit operator AgentProperty(string value) => new AgentProperty(value);
        public static implicit operator AgentProperty(Action value) => new AgentProperty(value);

    }
}
