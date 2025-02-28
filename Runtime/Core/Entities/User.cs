using JetBrains.Annotations;
using RabbitWings.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RabbitWings.Goals;
using RabbitWings.Catalog;

namespace RabbitWings.Core
{

    [Serializable]
    public class User : UserInfo
    {
        private static User _current;
        public static User Current { 
            get { return _current; } 
            set {
                _current = value;
                OnCurrentUserLoggedIn.Invoke(_current);
            }
        }
        public static event Action<User> OnCurrentUserLoggedIn;
        public static Dictionary<string, int> CurrentTotalCounts { get; private set; }
        public static event Action<Dictionary<string, int>> OnTotalsUpdated;

        public static Dictionary<string, int> UpdateDiffDictionaries(Dictionary<string, int> original, Dictionary<string, int> update, bool isFull)
        {
            Dictionary<string, int> diff = new Dictionary<string, int>();
            foreach (string i in update.Keys)
            {
                if (!original.ContainsKey(i))
                {
                    if (diff.ContainsKey(i)) { diff[i] += update[i]; } else { diff.Add(i, update[i]); }
                    original.Add(i, update[i]);
                    continue;
                }
                if (original[i] == update[i]) { continue; }
                if (diff.ContainsKey(i)) { diff[i] += (update[i] - original[i]); } else { diff.Add(i, (update[i] - original[i])); }
                original[i] = update[i];
                if (original[i] <= 0)
                {
                    original.Remove(i);
                }
            }
            if (isFull)
            {
                foreach (string i in original.Keys)
                {
                    if (!update.ContainsKey(i))
                    {
                        diff.Add(i, 0 - original[i]);
                        original.Remove(i);
                    }
                }
            }
            return diff;
        }
        public static void UpdateTotalCounts(Dictionary<string, int> newTotals, bool fullRefresh = false)
        {
            if(CurrentTotalCounts == null)
            {
                CurrentTotalCounts = new Dictionary<string, int>();
            }
            Dictionary<string, int> diff = UpdateDiffDictionaries(CurrentTotalCounts, newTotals, fullRefresh);
            OnTotalsUpdated?.Invoke(diff);
        }

        public void UpdateItems(Dictionary<string, int> newItems, bool fullRefresh = false)
        {
            Dictionary<string, int> diff = UpdateDiffItems(newItems, fullRefresh);
            OnItemsUpdated?.Invoke(diff);
        }
        public event Action<Dictionary<string, int>> OnItemsUpdated;
        public List<VirtualCurrencyBalance> vcurrencyBalances = new List<VirtualCurrencyBalance>();
        public Dictionary<string, int> itemCounts = new Dictionary<string, int>();
        public GoalItemManager goalItems = new GoalItemManager();
        public GoalStateSummary goals = new GoalStateSummary(1);

        protected Dictionary<string, int> UpdateDiffItems(Dictionary<string, int> update, bool isFull = false)
        {
            Dictionary<string, int> diff = new Dictionary<string, int>();
            foreach (string i in update.Keys)
            {
                if (ItemCache.ItemHolder.GetBundleBySku(i) != null) continue;
                InventoryItem it = ItemCache.ItemHolder.GetItemBySku(i);
                if (it == null)
                {
                    XDebug.LogException(new NullReferenceException($"Not sure what item this is: {i}"));
                    continue;
                }
                else
                {
                    if (it.VirtualItemType == VirtualItemType.VirtualCurrency)
                    {
                        if (update[i] == vcurrencyBalances[0].amount) { continue; }
                        diff.Add(i, update[i] - vcurrencyBalances[0].amount);
                        vcurrencyBalances[0].amount = update[i];
                        continue;
                    }
                    if(it.VirtualItemType == VirtualItemType.Hint)
                    {
                        goalItems.AddItem(it.GoalID, it);
                    }
                }
                if (!itemCounts.ContainsKey(i))
                {
                    if (diff.ContainsKey(i)) { diff[i] += update[i]; } else { diff.Add(i, update[i]); }
                    itemCounts.Add(i, update[i]);
                    continue;
                }
                if (itemCounts[i] == update[i]) { continue; }
                if (diff.ContainsKey(i)) { diff[i] += (update[i] - itemCounts[i]); } else { diff.Add(i, (update[i] - itemCounts[i])); }
                itemCounts[i] = update[i];
                if (itemCounts[i] <= 0)
                {
                    itemCounts.Remove(i);
                }
            }
            if (isFull)
            {
                foreach (string i in itemCounts.Keys)
                {
                    if (!update.ContainsKey(i))
                    {
                        diff.Add(i, 0 - itemCounts[i]);
                        itemCounts.Remove(i);
                    }
                }
            }
            return diff;
        }
    }
}
