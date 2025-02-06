using RabbitWings.Core;
using RabbitWings.UserAccount;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RabbitWings.Inventory
{
    public class InventoryCache : GenericCache<UserInventory>
    {
        protected string GetID(UserInventory current)
        {
            return $"{GetIDPrefix}-{current.email}";
        }
        protected string GetID(User current)
        {
            return $"{GetIDPrefix}-{current.email}";
        }
        public void GetCurrentUserInventory()
        {
            GetCurrentUserInventory((UserInventory ui) => { }, (Error e) => { });
        }
        public void GetCurrentUserInventory(Action<UserInventory> onComplete, Action<Error> onError)
        {
            string id = (UserInventory.Current == null ? GetID(User.Current) : GetID(UserInventory.Current));
            GetCache(id, (UserInventory ui) => {
                UserInventory.Current = ui;
                onComplete?.Invoke(ui);
            }, (Error e) => {
                onError?.Invoke(e);
            });
        }
        public void CacheCurrentUserInventory()
        {
            if (UserInventory.Current == null) { return; }
            CacheCurrentUserInventory((UserInventory u) => {
                Debug.Log($"User cached: {u}");
            }, (Error e) => {
                Debug.LogError($"Error caching user: {e}");
            });
        }
        public void CacheCurrentUserInventory(Action<UserInventory> onComplete, Action<Error> onError)
        {
            if (UserInventory.Current == null)
            {
                onError?.Invoke(new Error(ErrorType.UnknownError, "404", "404", "Current User doesn't exist yet."));
                return;
            }
            SetCache(GetID(UserInventory.Current), UserInventory.Current, (UserInventory usr) => {
                onComplete?.Invoke(UserInventory.Current);
            }, onError);
        }
    }
}