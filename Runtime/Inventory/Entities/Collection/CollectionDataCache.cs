using Newtonsoft.Json;
using RabbitWings.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RabbitWings.Inventory
{
    public class CollectionDataCache : GenericMissionNetCache<CollectionData, CollectionCountManager, MissionCollectionData, SKUCount, List<SKUCount>>
    {
    }
}
