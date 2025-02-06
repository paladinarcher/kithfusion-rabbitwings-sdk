using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RabbitWings.Inventory
{
    public class UserInventory
    {
        public static UserInventory Current { get; set; }
        public string email;
        public List<InventoryItemCount> items;
    }
}