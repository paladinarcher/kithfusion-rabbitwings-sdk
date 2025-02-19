using RabbitWings.Core;
using RabbitWings.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RabbitWings.Catalog
{
    [Serializable]
    public class ItemCacheMapHolder
    {
        public Dictionary<string, StoreItem> storeItemCache { get; set; }
        public Dictionary<string, BundleItem> bundleItemCache { get; set; }
        public GoalItemManager GoalItems { get; set; }
        public ItemCacheMapHolder()
        {
            storeItemCache = new Dictionary<string, StoreItem>();
            bundleItemCache = new Dictionary<string, BundleItem>();
            GoalItems = new GoalItemManager();
        }

        public InventoryItem GetItemBySku(string sku)
        {
            if (storeItemCache.ContainsKey(sku))
            {
                return storeItemCache[sku].InventoryItem;
            }
            return null;
        }

        public BundleItem GetBundleBySku(string sku)
        {
            if (bundleItemCache.ContainsKey(sku)) return bundleItemCache[sku];
            return null;
        }

        public StoreItem GetStoreItemBySku(string sku)
        {
            if (storeItemCache.ContainsKey(sku)) return storeItemCache[sku];
            return null;
        }
    }
}