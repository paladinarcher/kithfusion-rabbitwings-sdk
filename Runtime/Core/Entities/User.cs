using JetBrains.Annotations;
using RabbitWings.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RabbitWings.Goals;

namespace RabbitWings.Core
{

    [Serializable]
    public class User : UserInfo
    {
        public static User Current { get; set; }
        public List<VirtualCurrencyBalance> vcurrencyBalances = new List<VirtualCurrencyBalance>();
        public List<InventoryItem> inventoryItems = new List<InventoryItem>();
        public GoalItemManager goalItems = new GoalItemManager();
        public GoalStateSummary goalSummary = new GoalStateSummary(5);
        public InventoryItems InventoryItems
        {
            get
            {
                return new InventoryItems
                {
                    items = inventoryItems.ToArray(),
                };
            }
        }
    }
}
