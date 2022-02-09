﻿using Source;
using Source.Items;
using System;
using System.Linq;
using static War3Api.Blizzard;
using static War3Api.Common;

namespace Source.Behaviors
{
    public class Shop : TargetBehavior<unit>
    {

        public const int SHOP_RADIUS = 4000;

        private OrderChain orders;
        public override string GetStatusGerund()
        {
            return $"shopping at a {Target.GetName()}";
        }

        protected override int GetTargetTimeout()
        {
            return 30;
        }

        public override void Start()
        {
            base.Start();
            int bought = 0;
            Action action = () =>
            {
                var wanted = AI.GetWantedItems();
                //Console.WriteLine($"Wanted: {string.Join(", ", wanted)}");
                foreach (int itemID in wanted)
                {
                    if (!Target.IsSelling(itemID)) continue;
                    if (AI.TryPurchase(itemID, Target)) bought++;
                }
            };
            orders =
                new OrderMove(Target.GetLocation(), OrderMove.BUILDING_RADIUS)
                .Init(AI)
                .Then(new OrderEnter(Target))
                .Then(new OrderDo(action))
                .Then(new OrderWait(bought + 2))
                .Then(new OrderExit())
                ;
            //Console.WriteLine($"{AI.Unit.GetName()} shopping at {Target.GetName()}");
        }

        public override void Stop()
        {
            base.Stop();
            orders = null;
            // In case interrupted
            ShowUnitShow(AI.Unit);
        }

        public override bool Update()
        {
            return base.Update() && orders.Update();
        }

        protected override bool IsTargetStillValid(unit target)
        {
            return base.IsTargetStillValid(target) && !Target.IsDead();
        }

        protected override unit SelectTarget()
        {
            var shops = GetUnitsInRangeOfLocAll(SHOP_RADIUS, AI.Unit.GetLocation()).ToList()
                .Where(u => u.IsShop() && u.GetPlayer() == AI.HumanPlayer)
                .ToList();

            foreach (int itemID in AI.GetWantedItems())
            {
                //Console.WriteLine($"Looking to buy {itemID} from {shops.Count} shops");

                // If there are no shops that can sell this item, skip it
                var selling = shops.Where(shop => shop.IsSelling(itemID));
                if (!selling.Any()) continue;

                // But if I just don't have enough money, save until I do
                if (AI.Gold < Items.Items.GetItemCost(itemID)) return null;

                unit targetShop = selling.OrderBy(shop => shop.DistanceTo(AI.Unit)).First();
                //Console.WriteLine($"Found item #{itemID} at {targetShop.GetName()}");
                return targetShop;
            }
            return null;
        }
    }
}
