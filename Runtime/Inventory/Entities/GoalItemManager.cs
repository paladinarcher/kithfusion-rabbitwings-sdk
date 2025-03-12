using Newtonsoft.Json;
using RabbitWings.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RabbitWings.Inventory
{
    [Serializable]
    public class GoalItemManager
    {
        [NonSerialized] public UnityEvent onInitialize;

        [JsonProperty]
        protected Dictionary<int, List<InventoryItem>> list;
        public GoalItemManager()
        {
            list = new Dictionary<int, List<InventoryItem>>();
            Initialize();
        }

        protected void Initialize()
        {
            onInitialize?.Invoke();
        }

        public int Count
        {
            get
            {
                return list.Count;
            }
        }

        public List<InventoryItem> GetItems(int goalId)
        {
            if (!list.ContainsKey(goalId)) { list.Add(goalId, new List<InventoryItem>()); }
            return list[goalId];
        }

        public void AddItem(int goalId, InventoryItem item)
        {
            List<InventoryItem> l = GetItems(goalId);
            if (!l.Contains(item))
            {
                l.Add(item);
                l.Sort((InventoryItem a, InventoryItem b) => {
                    string sa = "ZZZ";
                    string sb = "ZZZ";
                    foreach (StoreItemAttribute sia in a.attributes)
                    {
                        if (sia.external_id != Constants.ATTRIBUTE_GOAL_ITEM_ORDER) { continue; }
                        sa = sia.external_id;
                    }
                    foreach (StoreItemAttribute sib in b.attributes)
                    {
                        if (sib.external_id != Constants.ATTRIBUTE_GOAL_ITEM_ORDER) { continue; }
                        sb = sib.external_id;
                    }
                    if (sa != sb) { return sa.CompareTo(sb); }
                    return a.sku.CompareTo(b.sku);
                });
                //Debug.Log($"GoalItemManager.AddItem: {goalId} {item.name}");
            }
        }
    }
}
