﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static War3Api.Common;
using static War3Api.Blizzard;
using Source;

namespace Source.Items
{
    public static class Items
    {
        private static readonly Dictionary<int, int> itemToResearchMap = new Dictionary<int, int>()
        {
            { Constants.ITEM_HEALING_POTION_LEVEL_1, Constants.UPGRADE_HEALTH_POTIONS },
        };
        private static readonly Dictionary<int, int> itemToSellerMap = new Dictionary<int, int>()
        {
            { Constants.ITEM_HEALING_POTION_LEVEL_1, Constants.UNIT_MARKET },
        };
        private static readonly List<int> itemIDs = new List<int>(itemToResearchMap.Keys);
        private static readonly List<int> shopIDs = new List<int>
        {
            Constants.UNIT_MARKET,
        };
        private static readonly Dictionary<int, int> itemCostMap = new Dictionary<int, int>();
        
        static Items()
        {
            foreach (int itemID in itemIDs)
            {
                item item = CreateItem(itemID, 0, 0);
                int cost = BlzGetItemIntegerField(item, ITEM_IF_PRIORITY);
                itemCostMap[itemID] = cost;
                RemoveItem(item);
            }
        }

        public static bool IsShop(this unit unit)
        {
            return shopIDs.Contains(GetUnitTypeId(unit));
        }

        public static bool IsSelling(this unit unit, int itemID)
        {
            if (!itemToSellerMap.TryGetValue(itemID, out int sellingID)) return false;
            if (sellingID != unit.GetTypeID()) return false;
            player player = unit.GetPlayer().GetHumanForAI();
            if (itemToSellerMap.TryGetValue(itemID, out int researchID))
            {
                // TODO: Account for research levels?
                if (GetPlayerTechResearched(player, researchID, false)) return false;
            }
            return true;
        }

        public static bool HasItem(this unit unit, int itemID)
        {
            return UnitHasItemOfTypeBJ(unit, itemID);
        }

        public static int GetItemCount(this unit unit, int itemID)
        {
            if (!UnitHasItemOfTypeBJ(unit, itemID)) return 0;
            item item = GetItemOfTypeFromUnitBJ(unit, itemID);
            return GetItemCharges(item);
        }

        public static int GetItemCost(int itemID)
        {
            if (itemCostMap.TryGetValue(itemID, out int cost)) return cost;
            return -1;
        }
    }
}
