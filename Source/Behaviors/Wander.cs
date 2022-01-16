using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static War3Api.Common;
using static War3Api.Blizzard;
using Source.Behaviors;

namespace Source.Behaviors
{
    public class Wander : TargetBehavior<location>
    {
        public const int WANDER_SIZE = 1000;

        private OrderChain orders;

        protected override int GetTargetTimeout()
        {
            return 20;
        }

        public override void Start()
        {
            base.Start();
            //Console.WriteLine($"Started wandering for {AI.Unit.GetName()}");
            orders = new OrderMove(Target, 100).Init(AI);
        }

        public override bool Update()
        {
            if (!base.Update()) return false;
            return orders.Update();
        }

        protected override location SelectTarget()
        {
            rect bounds = GetPlayableMapRect();
            rect around = RectFromCenterSizeBJ(AI.Unit.GetLocation(), WANDER_SIZE, WANDER_SIZE);
            location target = GetRandomLocInRect(around);
            if (!RectContainsLoc(bounds, target)) return null;
            return target;
        }
    }
}
