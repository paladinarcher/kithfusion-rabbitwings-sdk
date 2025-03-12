using RabbitWings.Core;
using RabbitWings.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RabbitWings.Catalog
{
    public class ItemMapCache : GenericCache<ItemCacheMapHolder>
    {
        public string MainItemCacheName = "MainItemCacheMap";
        public static ItemCacheMapHolder ItemHolder { get; set; }
        public static event Action OnItemHolderLoad;

        public void SetCache(Action<ItemCacheMapHolder> onComplete, Action<Error> onError)
        {
            if (ItemHolder == null)
            {
                onError.Invoke(new Error(ErrorType.InvalidData, "", "", "No Item Cache available to cache"));
                return;
            }

            SetCache(MainItemCacheName, ItemHolder, onComplete, onError);
        }

        public void GetCache(Action<ItemCacheMapHolder> onComplete, Action<Error> onError)
        {
            GetCache(MainItemCacheName, (ItemCacheMapHolder i) => {
                ItemHolder = i;
                if (ItemHolder.GoalItems.Count > 0) ItemHolder.GoalItems = new GoalItemManager();
                foreach (StoreItem item in ItemHolder.storeItemCache.Values)
                {
                    if(item.VirtualItemType != VirtualItemType.Hint) { continue; }
                    InventoryItem it = item.InventoryItem;
                    ItemHolder.GoalItems.AddItem(it.GoalID, it);
                }
                OnItemHolderLoad?.Invoke(); onComplete?.Invoke(i);
            }, onError);
        }
    }
}