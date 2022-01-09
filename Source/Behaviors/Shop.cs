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

            Console.WriteLine($"{AI.Unit.GetName()} shopping at {targetShop.GetName()}");
        }

        public override void Stop()
        {
            targetShop = null;
            orders = null;
            // In case interrupted
            ShowUnitShow(AI.Unit);
        }

        public override bool Update()
        {
            if (!IsValidTarget())
            {
                Console.WriteLine("Shop target became invalid");
                return false;
            }

            // TODO: Check for danger...

            return orders.Update();
        }

        private bool IsValidTarget()
        {
            return orders != null && !targetShop.IsDead();
        }

        protected virtual void SelectTarget()
        {
            if (IsValidTarget()) return;

            targetShop = null;

            var shops = GetUnitsInRangeOfLocAll(SHOP_RADIUS, AI.Unit.GetLocation()).ToList()
                .Where(u => u.IsShop() && u.GetPlayer() == AI.HumanPlayer)
                .ToList();

            foreach (int itemID in AI.GetWantedItems())
            {
                if (AI.Gold < Items.Items.GetItemCost(itemID)) continue;
                //Console.WriteLine($"Looking to buy {itemID} from {shops.Count} shops");
                var selling = shops.Where(shop => shop.IsSelling(itemID));
                if (selling.Count() == 0) continue;
                targetShop = selling.OrderBy(shop => shop.DistanceTo(AI.Unit)).First();
                Console.WriteLine($"Found item #{itemID} at {targetShop.GetName()}");
                break;
            }

            int bought = 0;
            Action action = () =>
            {
                foreach (int itemID in AI.GetWantedItems())
                {
                    if (!targetShop.IsSelling(itemID)) continue;
                    if (AI.TryPurchase(itemID)) bought++;
                }
            };
            orders =
                new OrderMove(targetShop.GetLocation(), OrderMove.BUILDING_RADIUS)
                .Init(AI)
                .Then(new OrderEnter(targetShop))
                .Then(new OrderDo(action))
                .Then(new OrderWait(bought + 2))
                .Then(new OrderExit())
                ;
            //Console.WriteLine($"Creating orders: {orders}");
        }
    }
}
