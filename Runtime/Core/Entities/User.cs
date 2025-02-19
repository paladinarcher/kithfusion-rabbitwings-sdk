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
        public Dictionary<string, int> itemCounts = new Dictionary<string, int>();
        public GoalItemManager goalItems = new GoalItemManager();
        public GoalStateSummary goalSummary = new GoalStateSummary(5);
    }
}
