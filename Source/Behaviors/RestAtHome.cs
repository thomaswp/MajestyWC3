using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source.Behaviors
{
    public class RestAtHome : Behavior
    {
        protected OrderChain orders;

        public override string GetStatusGerund()
        {
            return $"resting at home";
        }

        public override bool CanStart()
        {
            return true;
        }

        public override float StartWeight()
        {
            if (!CanStart()) return 0;
            return Util.lerp(1 - AI.Unit.GetHPFraction(), 1, 0.2f);
        }

        protected virtual bool ShouldExit()
        {
            return true;
        }

        protected virtual void OnReturned()
        {

        }

        public override void Start()
        {
            orders = new OrderMove(AI.Home.GetLocation(), OrderMove.BUILDING_RADIUS)
                .Init(AI)
                .Then(new OrderEnter(AI.Home))
                .Then(new OrderDo(() => OnReturned()))
                .Then(new OrderWaitUntil(() => AI.Unit.GetHPFraction() >= 0.99))
                .Then(new OrderWait(3))
                .Then(new OrderWaitUntil(() => ShouldExit()))
                .Then(new OrderExit())
                ;
        }

        public override void Stop()
        {
            AI.ExitBuilding();
        }

        public override bool Update()
        {
            if (orders == null) return false;
            return orders.Update();
        }
    }
}
