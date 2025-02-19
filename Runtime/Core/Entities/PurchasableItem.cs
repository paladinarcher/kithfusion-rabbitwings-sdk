using Newtonsoft.Json;
using RabbitWings.Inventory;
using System;

namespace RabbitWings.Core
{
    [Serializable]
    public class PurchasableItem
    {
        public string sku;
        public string name;
        public StoreItemGroup[] groups;
        public StoreItemAttribute[] attributes;
        public string type;
        public string description;
        public string image_url;
        public bool is_free;
        public Price price;
        public VirtualPrice[] virtual_prices;
        public StoreItemPromotion[] promotions;
        public StoreItemLimits limits;
    }
}