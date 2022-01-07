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
        public bool Done { get; private set; }
        private OrderChain next;
        private bool started = false;

        protected virtual bool UpdateContinue() { return false; }
        protected virtual void Start() { }

        public bool Update()
        {
            if (!started)
            {
                Start();
                started = true;
            }
            Done |= !UpdateContinue();
            if (!Done) return true;
            return next == null || next.Update();
        }

        public OrderChain Then(OrderChain next)
        {
            if (this.next != null)
            {
                next.Then(next);
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
    }

    public abstract class OrderMove : OrderChain
    {
        private location destination;
        private float threshold;

        public OrderMove(location destination, float threshold)
        {
            this.destination = destination;
            this.threshold = threshold;
        }

        protected override void Start()
        {
            base.Start();
            AI.Unit.OrderMoveTo(destination);
        }

        protected override bool UpdateContinue()
        {
            return DistanceBetweenPoints(AI.Unit.GetLocation(), destination) > threshold;
        }
    }

    public class OrderMoveToBuilding : OrderMove
    {
        public OrderMoveToBuilding(unit building)
            : base(building.GetLocation(), BlzGetUnitCollisionSize(building) + 30)
        { }
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
            ShowUnitHide(AI.Unit);
            AI.InBuilding = building;
        }
    }

    public class OrderExit : OrderChain
    { 
        protected override void Start()
        {
            // TODO: move to bottom of building?
            unit building = AI.InBuilding;
            AI.InBuilding = null;
            ShowUnitShow(AI.Unit);
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
            var timer = CreateTimer();
            TimerStart(timer, seconds, false, () =>
            {
                DestroyTimer(timer);
                done = true;
            });
        }

        protected override bool UpdateContinue()
        {
            return done;
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
