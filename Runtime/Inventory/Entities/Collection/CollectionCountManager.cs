using RabbitWings.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RabbitWings.Inventory
{
    public class CollectionCountManager : AbstractMissionNetCacheManager<CollectionData, MissionCollectionData, SKUCount, List<SKUCount>>
    {
        protected CollectionData myData;

        public override CollectionData Data
        {
            get
            {
                if (myData == null)
                {
                    myData = new CollectionData();
                }
                return myData;
            }
            set
            {
                if (myData == null)
                {
                    myData = value;
                }
                else
                {
                    myData.Merge(value);
                }
            }
        }


        public void Add(InventoryItem i) {
            Data.Add(i);
        }
        public int GetCount(InventoryItem item)
        {
            return GetCount(item.sku);
        }
        public int GetCount(string sku)
        {
            MissionCollectionData c = Data.GetMissionData();
            if (c == null)
            {
                return 0;
            }
            return c.GetCountBySKU(sku).count;
        }

        public override void Add(SKUCount bit)
        {
            Data.Add(bit);
        }
    }
}
