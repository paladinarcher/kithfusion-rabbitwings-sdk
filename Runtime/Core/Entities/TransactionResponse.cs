using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RabbitWings.Core
{
    [Serializable]
    public class TransactionResponse 
    {
        public Dictionary<string, int> totalCounts;
        public Dictionary<string, int> inventory;
    }
}
