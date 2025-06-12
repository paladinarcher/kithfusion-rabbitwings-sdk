using RabbitWings.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RabbitWings.Inventory
{

    [Serializable]
    public class MissionCollectionData : ByMissionAndNetData<SKUCount>
    {
        public void Add(InventoryItem item)
        {
            SKUCount c = GetCountBySKU(item.sku);
            int b = c.count;
            c += item.quantity;
            if (c.count != b) IsDirty = true;
        }
        public SKUCount GetCountBySKU(string sku)
        {
            foreach (SKUCount item in Data)
            {
                if (item.sku == sku) { return item; }
            }
            SKUCount c = new()
            {
                sku = sku,
                count = 0
            };
            Add(c);
            return c;
        }

        public override bool Merge(SKUCount existingItem, SKUCount newItem)
        {
            if (existingItem.count != newItem.count)
            {
                existingItem.count += newItem.count;
                return true;
            }
            return false;
        }
    }
}