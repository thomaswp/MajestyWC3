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

        public override bool CanStart()
        {
            SelectTarget();
            return targetShop != null;
        }

        public override void Start()
        {
            SelectTarget();
            if (targetShop == null) return;
            IssuePointOrderByIdLoc(AI.unit, Constants.ORDER_MOVE, targetShop.GetLocation());
            Console.WriteLine($"{AI.unit.GetName()} shopping at {targetShop.GetName()}");
        }

        public override void Stop()
        {
            targetShop = null;
        }

        public override bool Update()
        {
            if (!IsValidTarget()) return false;

            // TODO: Check for danger...
            //int totalEnemyHP = visible.Where(u => !u.IsStructure()).Select(u => u.GetHP()).Sum();

            return true;
        }

        private bool IsValidTarget()
        {
            // TODO
            return true;
        }

        protected virtual void SelectTarget()
        {
            if (IsValidTarget()) return;

            targetShop = null;

            var shops = GetUnitsInRangeOfLocAll(SHOP_RADIUS, AI.unit.GetLocation()).ToList()
                .Where(u => u.IsShop() && u.GetPlayer() == AI.humanPlayer);

            foreach (int itemID in AI.GetWantedItems())
            {
                // TODO Get Item cost
                //if (AI.gold < Itemcost) continue;
                var selling = shops.Where(shop => shop.IsSelling(itemID));
                if (selling.Count() == 0) continue;
                targetShop = selling.OrderBy(shop => shop.DistanceTo(AI.unit)).First();
                break;
            }

            if (targetShop != null)
            {
                // TODO: Go to shop
            }
        }
    }
}
