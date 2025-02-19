using RabbitWings.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RabbitWings.Catalog
{
    public class ItemCache : ItemMapCache//GenericCache<ItemCacheHolder>
    {
        //public string MainItemCacheName = "MainItemCache";
        //public static ItemCacheHolder ItemHolder { get; set; }

        //public void SetCache(Action<ItemCacheHolder> onComplete, Action<Error> onError)
        //{
        //    if (ItemHolder == null) {
        //        onError.Invoke(new Error(ErrorType.InvalidData, "", "", "No Item Cache available to cache"));
        //        return;
        //    }

        //    SetCache(MainItemCacheName, ItemHolder, onComplete, onError);
        //}

        //public void GetCache(Action<ItemCacheHolder> onComplete, Action<Error> onError)
        //{
        //    GetCache(MainItemCacheName, (ItemCacheHolder i) => {
        //        ItemHolder = i; onComplete.Invoke(i);
        //    }, onError);
        //}
    }
}
