using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static War3Api.Common;
using static War3Api.Blizzard;
using Source;
using Source.Units;

namespace Source.Items
{



    public static class Items
    {
        public static readonly int HP1_HEALING, HP2_HEALING;

        static Items()
        {
            item hp1 = CreateItem(Constants.ITEM_HEALING_POTION_LEVEL_1, 0, 0);
            HP1_HEALING = BlzGetAbilityIntegerLevelField(
                BlzGetItemAbility(hp1, Constants.ABILITY_ITEM_HEALING_LEVEL_1),
                ABILITY_ILF_HIT_POINTS_GAINED_IHP2, 1);
            RemoveItem(hp1);

            item hp2 = CreateItem(Constants.ITEM_HEALING_POTION_LEVEL_2, 0, 0);
            HP2_HEALING = BlzGetAbilityIntegerLevelField(
                BlzGetItemAbility(hp2, Constants.ABILITY_ITEM_HEALING_LEVEL_2),
                ABILITY_ILF_HIT_POINTS_GAINED_IHP2, 1);
            RemoveItem(hp2);
        }

        //public static int[] GetUpgradeChain(int itemID)
        //{
        //    if (!upgradeMap.TryGetValue(itemID, out int[] chain)) return new int[] { itemID };
        //    return chain;
        //}

        public static bool IsShop(this unit unit)
        {
            return ShopInfo.IsShop(unit.GetTypeID());
        }

        public static bool IsSelling(this unit unit, int itemID)
        {
            ItemInfo info = itemID;
            if (info == null) return false;
            if (!info.IsSoldBy(unit.GetTypeID())) return false;
            player player = unit.GetPlayer().GetHumanForAI();
            int researchLevel = GetPlayerTechCount(player, info.Requirement.upgradeID, true);
            //Console.WriteLine($"Player research level {researchLevel}; required: {researchReq.level}");
            if (researchLevel < info.Requirement.level) return false;
            return true;
        }

        public static bool HasExactItem(this unit unit, int itemID)
        {
            return UnitHasItemOfTypeBJ(unit, itemID);
        }

        public static int[] GetItemIDAndUpgrades(int itemID)
        {
            ItemInfo info = itemID;
            if (info == null) return new int[] { itemID };
            return info.UpgradeChain;
        }

        public static IEnumerable<int> GetPriorUpgrades(int itemID)
        {
            ItemInfo info = itemID;
            if (info == null) yield break;
            int[] chain = info.UpgradeChain;
            for (int i = 0; i < chain.Length; i++)
            {
                if (chain[i] == itemID) break;
                yield return chain[i];
            }
        }

        public static bool HasItemOrUpgrade(this unit unit, int itemID)
        {
            return GetItemIDAndUpgrades(itemID).Any(id => unit.HasExactItem(id));
        }

        public static int GetExactItemCount(this unit unit, int itemID)
        {
            if (!UnitHasItemOfTypeBJ(unit, itemID)) return 0;
            item item = GetItemOfTypeFromUnitBJ(unit, itemID);
            return Math.Max(1, GetItemCharges(item));
        }

        public static int GetItemOrUpgradeCount(this unit unit, int itemID)
        {
            return GetItemIDAndUpgrades(itemID).Sum(id => unit.GetExactItemCount(id));
        }

        public static void RemoveReplacedItems(this unit unit, int itemID)
        {
            if (itemID == Constants.ITEM_HEALING_POTION_LEVEL_1 ||
                itemID == Constants.ITEM_HEALING_POTION_LEVEL_2) return;

            foreach (int id in GetPriorUpgrades(itemID))
            {
                Console.WriteLine($"Attempting to remove item {id} from {unit.GetName()}");
                item item = GetItemOfTypeFromUnitBJ(unit, id);
                if (item == null) continue; 
                RemoveItem(item);
                //int slot = unit.GetSlotOrderForItem(id);
                //if (slot < 0) continue;
                //UnitRemoveItemFromSlot(unit, slot);
            }
        }

        public static int GetItemCost(int itemID)
        {
            ItemInfo info = itemID;
            if (info == null) return -1;
            return info.Cost;
        }

        public static readonly int[] SLOTS_ORDERS = new int[] {
            Constants.ORDER_USE_SLOT1,
            Constants.ORDER_USE_SLOT2,
            Constants.ORDER_USE_SLOT3,
            Constants.ORDER_USE_SLOT4,
            Constants.ORDER_USE_SLOT5,
            Constants.ORDER_USE_SLOT6,
        };

        public static int GetSlotOrderForItem(this unit unit, int itemID)
        {
            for (int i = 1; i <= 6; i++)
            {
                item item = UnitItemInSlot(unit, i);
                if (item != null && GetItemTypeId(item) == itemID)
                {
                    return SLOTS_ORDERS[i - 1];
                }
            }
            return -1;
        }
    }
}
