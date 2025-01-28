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
    }
}