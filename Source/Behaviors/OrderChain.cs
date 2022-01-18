using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static War3Api.Common;
using static War3Api.Blizzard;
using Source.Behaviors;
using Source.Units;

namespace Source.Behaviors
{
    public abstract class OrderChain
    {
        public UnitAI AI { get; private set; }
        private OrderChain next;

        private bool started = false;
        private bool done;

        protected virtual bool UpdateContinue() { return false; }
        protected virtual void Start() 
        {
            //Console.WriteLine($"{AI.Unit.GetName()} starting {GetType().Name}");
        }

        public bool Update()
        {
            if (!started)
            {
                Start();
                started = true;
            }
            if (!done)
            {
                done |= !UpdateContinue();
            }
            if (!done) return true;
            if (next == null) return false;
            return next.Update();
        }

        public bool IsDone()
        {
            return done && (next == null || next.IsDone());
        }

        public OrderChain Then(OrderChain next)
        {
            if (this.next != null)
            {
                this.next.Then(next);
            }
            else
            {
                this.next = next;
                next.AI = AI;
            }
            return this;
        }

        public OrderChain Init(UnitAI ai)
        {
            AI = ai;
            if (next != null) next.Init(ai);
            return this;
        }

        public override string ToString()
        {
            string name = GetType().Name;
            if (next == null) return name;
            return $"{GetType().Name}->{next}";
        }
    }

    public class OrderMove : OrderChain
    {
        private readonly location destination;
        private readonly float thresholdRadius;

        public const int BUILDING_RADIUS = 260;
        float lastDistance;

        public OrderMove(location destination, float thresholdRadius)
        {
            this.destination = destination;
            this.thresholdRadius = thresholdRadius;
        }

        protected override void Start()
        {
            base.Start();
            AI.Unit.OrderMoveTo(destination);
            lastDistance = AI.Unit.DistanceTo(destination);
        }

        protected override bool UpdateContinue()
        {
            // Still moving
            if (GetUnitCurrentOrder(AI.Unit) == Constants.ORDER_MOVE) return true;
            float dis = AI.Unit.DistanceTo(destination);
            // We made it
            if (dis <= thresholdRadius) return false;
            // Haven't made it yet but still getting closer
            if (dis < lastDistance)
            {
                Start();
                return true;
            }
            // Stuck
            // TODO: Handle failed orders...
            Console.WriteLine($"{AI.Unit.GetName()} stuck, failed to move");
            PingMinimapLocForPlayer(GetLocalPlayer(), destination, 1);
            return false;
        }
    }

    public class OrderEnter : OrderChain
    {
        private unit building;

        public OrderEnter(unit building)
        {
            this.building = building;
        }

        protected override void Start()
        {
            base.Start();
            AI.EnterBuilding(building);
        }
    }

    public class OrderExit : OrderChain
    { 
        protected override void Start()
        {
            AI.ExitBuilding();
        }
    }

    public class OrderWait : OrderChain
    {
        private float seconds;
        private bool done = false;

        public OrderWait(float seconds)
        {
            this.seconds = seconds;
        }

        protected override void Start()
        {
            base.Start();
            var timer = CreateTimer();
            TimerStart(timer, seconds, false, () =>
            {
                DestroyTimer(timer);
                done = true;
            });
        }

        protected override bool UpdateContinue()
        {
            return !done;
        }
    }

    public class OrderWaitUntil : OrderChain
    {
        private Func<bool> predicate;

        public OrderWaitUntil(Func<bool> predicate)
        {
            this.predicate = predicate;
        }

        protected override bool UpdateContinue()
        {
            return !predicate();
        }
    }

    public class OrderDo : OrderChain
    {
        private Action action;

        public OrderDo(Action action)
        {
            this.action = action;
        }

        protected override void Start()
        {
            base.Start();
            action();
        }
    }
}
