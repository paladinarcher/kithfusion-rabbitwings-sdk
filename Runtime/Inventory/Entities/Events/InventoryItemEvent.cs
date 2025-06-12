using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RabbitWings.Inventory
{
    [Serializable]
    public class InventoryItemEvent : UnityEvent<InventoryItem>
    {

    }
}