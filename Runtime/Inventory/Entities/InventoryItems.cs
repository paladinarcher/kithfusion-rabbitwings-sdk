using System;
using RabbitWings.Core;
using UnityEngine.Events;

namespace RabbitWings.Inventory
{
	[Serializable]
	public class InventoryItems
	{
		public InventoryItem[] items;
	}

	[Serializable]
	public class InventoryItem
	{
		public string sku;
		public string name;
		public string description;
		//Public object attributes; Don't use it yet.
		public string type;
		public string virtual_item_type;
		public StoreItemAttribute[] attributes;
		public StoreItemGroup[] groups;
		public string image_url;
		public int quantity;
		public int? remaining_uses;
		public string instance_id;
        protected InventorySellDescriptor sellDescriptor;

		public VirtualItemType VirtualItemType
		{
			get
			{
				if (!string.IsNullOrEmpty(type) && type.Equals("virtual_currency"))
					return VirtualItemType.VirtualCurrency;

				if (string.IsNullOrEmpty(virtual_item_type))
					return VirtualItemType.None;

				switch (virtual_item_type)
				{
					case "consumable": return VirtualItemType.Consumable;
					case "non_consumable": return VirtualItemType.NonConsumable;
					case "non_renewing_subscription": return VirtualItemType.NonRenewingSubscription;
					default: return VirtualItemType.None;
				}
			}
		}
        public InventorySellDescriptor SellDescriptor()
        {
            return SellDescriptor(Constants.RIS_CURRENCY_SKU);
        }
        public InventorySellDescriptor SellDescriptor(string vcurrencySku)
        {
            if (sellDescriptor == null)
            {
                sellDescriptor = new InventorySellDescriptor();
                sellDescriptor.item = this;
                sellDescriptor.currenySku = vcurrencySku;
                sellDescriptor.currenctName = Constants.VCURENCY_SKU_TO_NAME[vcurrencySku];
                if (attributes == null || attributes.Length < 1)
                {
                    return sellDescriptor;
                }
                if (quantity < 1)
                {
                    return sellDescriptor;
                }
                int itemCurrencyValue = 0;
                for (int i = 0; i < attributes.Length; i++)
                {
                    if (attributes[i].external_id == Constants.CURRENCY_VALUE_ATTRIBUTE_ID)
                    {
                        if (attributes[i].values.Length < 1)
                        {
                            break;
                        }
                        for (int j = 0; j < attributes[i].values.Length; j++)
                        {
                            //values are in the format "ris_#" where # is the numerical exchange value
                            if (attributes[i].values[j].external_id.Contains(Constants.RIS_CURRENCY_VALUE_PREFIX))
                            {
                                if (Int32.TryParse(attributes[i].values[j].external_id.Substring(Constants.RIS_CURRENCY_VALUE_PREFIX.Length), out itemCurrencyValue))
                                {
                                    sellDescriptor.isSellable = true;
                                    sellDescriptor.exchangeRate = itemCurrencyValue;
                                    return sellDescriptor;
                                }
                            }
                        }

                        if (itemCurrencyValue == 0)
                        {
                            return sellDescriptor;
                        }
                    }
                }
            }
            return sellDescriptor;
        }
    }

    public class InventorySellDescriptor
    {
        public InventoryItem item;
        public bool isSellable = false;
        public string currenySku = Constants.RIS_CURRENCY_SKU;
        public string currenctName = Constants.RIS_CURRENCY_NAME;
        public int exchangeRate = 0;
    }

    [Serializable]
    public class AuthInventoryItemEvent : UnityEvent<User>
    {

    }
}