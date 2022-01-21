using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static War3Api.Common;
using static War3Api.Blizzard;

namespace Source
{
    public static class AnyUnitEvents
    {
        private static Dictionary<playerunitevent, EventHandler> anyUnitEvents = 
            new Dictionary<playerunitevent, EventHandler>();

        class EventHandler
        {
            public List<Action> actions = new List<Action>();
            playerunitevent @event;
            trigger trigger;

            public EventHandler(playerunitevent @event)
            {
                this.@event = @event;
                trigger = CreateTrigger();
                TriggerRegisterAnyUnitEventBJ(trigger, @event);
                TriggerAddAction(trigger, () => actions.ForEach(a => a()));
            }
        }

        public static void Register(playerunitevent @event, Action action)
        {
            if (!anyUnitEvents.TryGetValue(@event, out var handler))
            {
                anyUnitEvents[@event] = handler = new EventHandler(@event);
            }
            handler.actions.Add(action);
        }
    }
}
