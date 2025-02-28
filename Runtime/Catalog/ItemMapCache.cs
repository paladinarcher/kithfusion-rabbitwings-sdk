using RabbitWings.Core;
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
                ItemHolder = i; OnItemHolderLoad?.Invoke(); onComplete?.Invoke(i);
            }, onError);
        }
    }
}