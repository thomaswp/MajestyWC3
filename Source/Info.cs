using System;
using System.Collections.Generic;
using System.Linq;

using static War3Api.Common;
using static War3Api.Blizzard;

namespace Source
{

    public abstract class Info
    {
        public string Name { get { return GetObjectName(ID); } }

        public static void Init()
        {
            try
            {
                BuildingInfo.InitBuildings();
                ItemInfo.InitItems();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error initiating info: " + e.Message);
            }
        }

        public int ID;

        public static implicit operator int(Info info) => info == null ? -1 : info.ID;

        protected static void AddToMap<T>(Dictionary<int, T> map, List<T> list) where T : Info
        {
            //Console.WriteLine("Adding..");
            foreach (T info in list)
            {
                //Console.WriteLine("Adding: " + info.ToString());
                map.Add(info.ID, info);
            }
        }

        protected static void PrintList<T>(string header, List<T> list)
            where T : Info
        {
            Console.WriteLine(header);
            //Console.WriteLine(string.Join(",", map.Keys));
            foreach (Info i in list)
            {
                Console.WriteLine($"{i.ID} => {i}");
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public abstract class UpgradableInfo<T> : Info
        where T : UpgradableInfo<T>
    {
        public int UpgradeIndex;
        public T UpgradesTo, UpgradesFrom;
        public int[] UpgradeChain;

        public static List<Tx> AddChain<Tx>(List<Tx> chain) where Tx : UpgradableInfo<Tx>
        {
            int[] intChain = new int[chain.Count];
            for (int i = 0; i < chain.Count; i++)
            {
                Tx info = chain[i];
                intChain[i] = info;
                info.UpgradeIndex = i;
                info.UpgradeChain = intChain;
                if (i == 0) continue;
                info.UpgradesFrom = chain[i - 1];
                chain[i - 1].UpgradesTo = info;
            }
            return chain;
        }

        public override string ToString()
        {
            string s = base.ToString();
            if (UpgradesTo != null) s += " -> " + UpgradesTo.Name;
            return s;
        }
    }

    public class BuildingInfo : UpgradableInfo<BuildingInfo>
    {
        public bool CanBeTaxed = true;
        public float TaxRate = 0.4f;

        public static readonly List<BuildingInfo> All = new();
        public static Dictionary<int, BuildingInfo> Map = new();

        internal static void InitBuildings()
        {
            All.AddRange(new List<BuildingInfo>()
            {
                new()
                {
                    ID = Constants.UNIT_CASTLE_LEVEL_1,
                    CanBeTaxed = false,
                },

                new() { ID = Constants.UNIT_WARRIORS_BARRACKS, },
                new() { ID = Constants.UNIT_RANGERS_HALL, },
            });

            All.AddRange(AddChain(new List<BuildingInfo>() {
                new ShopInfo() { ID = Constants.UNIT_BLACKSMITH_LEVEL_1, },
                new ShopInfo() { ID = Constants.UNIT_BLACKSMITH_LEVEL_2, },
                new ShopInfo() { ID = Constants.UNIT_BLACKSMITH_LEVEL_3, },
            }));

            All.AddRange(AddChain(new List<BuildingInfo>() {
                new ShopInfo() { ID = Constants.UNIT_MARKET_LEVEL_1, },
                new ShopInfo() { ID = Constants.UNIT_MARKET_LEVEL_2, },
            }));

            //PrintList("Buildings", All);
            AddToMap(Map, All);
        }

        public static implicit operator BuildingInfo(int id) => Map.GetValue(id);
    }

    public class ShopInfo : BuildingInfo
    {
        public readonly List<int> ItemsSold = new();

        public static readonly List<BuildingInfo> AllShops =
            All.Where(b => b is ShopInfo).ToList();

        public static implicit operator ShopInfo(int id)
        {
            BuildingInfo info = Map.GetValue(id);
            //Console.WriteLine($"Lookup shop: {id} =>");
            //Console.WriteLine(info);
            if (info is ShopInfo) return (ShopInfo)info;
            return null;
        }

        public static bool IsShop(int typeID)
        {
            return (BuildingInfo)typeID is ShopInfo;
        }
    }

    public struct UpgradeRequirement
    {
        public int upgradeID, level;

        public UpgradeRequirement(int upgradeID, int level = 1)
        {
            this.upgradeID = upgradeID;
            this.level = level;
        }
    }

    public class ItemInfo : UpgradableInfo<ItemInfo>
    {
        public UpgradeRequirement Requirement;
        public int Seller;
        public int Cost;

        public static readonly List<ItemInfo> All = new();
        public static readonly Dictionary<int, ItemInfo> Map = new();

        internal static void InitItems()
        {
            AddItemChain(Constants.UNIT_MARKET_LEVEL_1, Constants.UPGRADE_HEALTH_POTIONS, false,
                Constants.ITEM_HEALING_POTION_LEVEL_1, Constants.ITEM_HEALING_POTION_LEVEL_2);

            AddItemChain(Constants.UNIT_BLACKSMITH_LEVEL_1, Constants.UPGRADE_LEVEL_3_WEAPONS, true,
                Constants.ITEM_SWORD_LEVEL_1,
                Constants.ITEM_SWORD_LEVEL_2
            );

            AddItemChain(Constants.UNIT_BLACKSMITH_LEVEL_1, Constants.UPGRADE_LEVEL_3_WEAPONS, true,
                Constants.ITEM_BOW_LEVEL_1
            );

            AddItemChain(Constants.UNIT_BLACKSMITH_LEVEL_1, Constants.UPGRADE_LEVEL_3_ARMOR, true,
                Constants.ITEM_PLATE_ARMOR_LEVEL_1
            );

            AddItemChain(Constants.UNIT_BLACKSMITH_LEVEL_1, Constants.UPGRADE_LEVEL_3_ARMOR, true,
                Constants.ITEM_HIDE_ARMOR_LEVEL_1,
                Constants.ITEM_HIDE_ARMOR_LEVEL_2
            );

            All.ForEach(i => ((ShopInfo)i.Seller).ItemsSold.Add(i));

            foreach (ItemInfo info in All)
            {
                item item = CreateItem(info, 0, 0);
                int cost = BlzGetItemIntegerField(item, ITEM_IF_PRIORITY);
                //Console.WriteLine(cost);
                info.Cost = cost;
                RemoveItem(item);
            }


            //PrintList("Items", All);
            AddToMap(Map, All);
        }

        public static implicit operator ItemInfo(int id) => Map.GetValue(id);

        private static void AddItemChain(int baseShop, int baseUpgrade, bool incrementUpgrade, params int[] ids)
        {
            ShopInfo shop = baseShop;
            List<ItemInfo> items = new();
            int upgradeLevel = 1;
            for (int i = 0; i < ids.Length; i++)
            {
                Console.WriteLine($"IU {i}: {shop}");
                items.Add(new()
                {
                    ID = ids[i],
                    Requirement = new UpgradeRequirement(baseUpgrade, upgradeLevel),
                    Seller = (int) shop, // Must cast to prevent transpilation bug
                });
                shop = (ShopInfo)shop.UpgradesTo;
                if (incrementUpgrade) upgradeLevel++;
            }
            All.AddRange(AddChain(items));
        }

        public bool IsSoldBy(ShopInfo info)
        {
            if (info == null) return false;
            if (Seller == info) return true;
            if (info.UpgradesFrom == null) return false;
            return IsSoldBy((ShopInfo)info.UpgradesFrom);
        }

        public override string ToString()
        {
            return $"{base.ToString()} ({Cost}g)";
        }
    }
}
