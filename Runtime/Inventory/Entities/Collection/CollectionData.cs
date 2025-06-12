using RabbitWings.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RabbitWings.Inventory
{
    [Serializable]
    public class CollectionData : MissionAndNetData<MissionCollectionData, SKUCount, List<SKUCount>>
    {
        public void Add(InventoryItem item)
        {
            Debug.Log($"CollectionData.Add {item.quantity} X {item.sku}");
            MissionCollectionData m = GetMissionData();
            if (m != null)
            {
                m.Add(item);
            }
        }
    }
}