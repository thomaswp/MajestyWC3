using Source.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static War3Api.Common;
using static War3Api.Blizzard;
using Source;
using Source.Items;

namespace Source.Behaviors
{
    public class Shop : Behavior
    {

        public const int SHOP_RADIUS = 4000;

        protected unit targetShop;

        private OrderChain orders;

        public override bool CanStart()
        {
            SelectTarget();
            return targetShop != null;
        }

        public override void Start()
        {
            SelectTarget();
            if (targetShop == null) return;
            IssuePointOrderByIdLoc(AI.Unit, Constants.ORDER_MOVE, targetShop.GetLocation());
            Console.WriteLine($"{AI.Unit.GetName()} shopping at {targetShop.GetName()}");
        }

        public override void Stop()
        {
            targetShop = null;
        }

        public override bool Update()
        {
            if (!IsValidTarget()) return false;

            // TODO: Check for danger...

            return orders.Update();
        }

        private bool IsValidTarget()
        {
            return orders != null && !targetShop.IsDead() && !orders.Done;
        }

        protected virtual void SelectTarget()
        {
            if (IsValidTarget()) return;

            targetShop = null;
            orders = null;

            var shops = GetUnitsInRangeOfLocAll(SHOP_RADIUS, AI.Unit.GetLocation()).ToList()
                .Where(u => u.IsShop() && u.GetPlayer() == AI.HumanPlayer);

            foreach (int itemID in AI.GetWantedItems())
            {
                if (AI.gold < Items.Items.GetItemCost(itemID)) continue;
                var selling = shops.Where(shop => shop.IsSelling(itemID));
                if (selling.Count() == 0) continue;
                targetShop = selling.OrderBy(shop => shop.DistanceTo(AI.Unit)).First();
                break;
            }

            if (targetShop != null)
            {
                Action action = () =>
                {
                    foreach (int itemID in AI.GetWantedItems())
                    {
                        if (!targetShop.IsSelling(itemID)) continue;
                        AI.TryPurchase(itemID);
                    }
                };
                orders =
                    new OrderMoveToBuilding(targetShop)
                    .Init(AI)
                    .Then(new OrderEnter(targetShop))
                    .Then(new OrderWait(1))
                    .Then(new OrderDo(action))
                    .Then(new OrderExit())
                    ;
                    
            }
        }
    }
}
