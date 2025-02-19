using RabbitWings.Core;
using RabbitWings.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RabbitWings.Catalog
{
    [Serializable]
    public class ItemCacheHolder
    {
        public List<InventoryItem> inventoryCache { get; set; }
        public List<StoreItem> storeItemCache { get; set; }
        public List<BundleItem> bundleItemCache { get; set; }
        public GoalItemManager GoalItems { get; set; }
        public ItemCacheHolder()
        {
            inventoryCache = new List<InventoryItem>();
            storeItemCache = new List<StoreItem>();
            bundleItemCache = new List<BundleItem>();
            GoalItems = new GoalItemManager();
        }

        public InventoryItem GetItemBySKU(string sku)
        {
            foreach (InventoryItem item in inventoryCache)
            {
                if (item.sku == sku) return item;
            }
            return null;
        }

        public BundleItem GetBundleBySku(string sku)
        {
            foreach (BundleItem bi in bundleItemCache)
            {
                if (bi.sku == sku) return bi;
            }
            return null;
        }
    }
}