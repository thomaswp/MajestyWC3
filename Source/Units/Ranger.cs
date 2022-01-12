using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static War3Api.Common;
using static War3Api.Blizzard;
using Source;

namespace Source.Units
{
    public class Ranger : Hero
    {
        public static List<int> itemPriorities = new List<int>()
        {
            Constants.ITEM_HEALING_POTION_LEVEL_2,
            Constants.ITEM_HEALING_POTION_LEVEL_2,
            Constants.ITEM_HEALING_POTION_LEVEL_2,
            Constants.ITEM_HEALING_POTION_LEVEL_2,
            Constants.ITEM_HEALING_POTION_LEVEL_2,
            Constants.ITEM_HEALING_POTION_LEVEL_1,
            Constants.ITEM_HEALING_POTION_LEVEL_1,
            Constants.ITEM_HEALING_POTION_LEVEL_1,
            Constants.ITEM_HEALING_POTION_LEVEL_1,
            Constants.ITEM_HEALING_POTION_LEVEL_1,
        };

        protected override IEnumerable<int> GetWantedItemsList()
        {
            return itemPriorities;
        }

        protected override void AddBehaviors()
        {
            AddBehavior(new Behaviors.Explore(), 5);
            AddBehavior(new Behaviors.BountyExploreFlag(), 5);
            AddBehavior(new Behaviors.DefendHome());
            AddBehavior(new Behaviors.Fight(), 4);
            AddBehavior(new Behaviors.Shop(), 5);
            AddBehavior(new Behaviors.RestAtHome(), 1);
            AddBehavior(new Behaviors.Flee(), 3);
        }

        protected override void Init(unit unit)
        {
            base.Init(unit);

            // TODO: remove
            //TryPurchase(Constants.ITEM_HEALING_POTION_LEVEL_1);
            //TryPurchase(Constants.ITEM_HEALING_POTION_LEVEL_1);
        }


    }
}
