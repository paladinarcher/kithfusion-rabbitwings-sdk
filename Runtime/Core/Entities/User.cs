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
        public List<VirtualCurrencyBalance> vcurrencyBalances;
        public List<InventoryItem> inventoryItems;
        public GoalItemManager goalItems;
        public GoalStateSummary goalSummary;
    }
}
