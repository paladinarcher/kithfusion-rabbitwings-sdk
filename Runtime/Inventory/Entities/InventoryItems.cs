using System;
using System.Collections.Generic;
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
    public class InventoryItemCount
    {
        public int quantity;
        public string sku;
    }

	[Serializable]
	public class InventoryItem
	{
        public static void CopyTo(InventoryItem original, out InventoryItem copy)
        {
            copy = new InventoryItem();
            copy.attributes = original.attributes;
            copy.sku = original.sku;
            copy.quantity = original.quantity;
            copy.description = original.description;
            copy.groups = original.groups;
            copy.image_url = original.image_url;
            copy.instance_id = original.instance_id;
            copy.name = original.name;
            copy.remaining_uses = original.remaining_uses;
            copy.type = original.type;
            copy.virtual_item_type = original.virtual_item_type;
        }
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
        public bool hidden = false;
        public bool syncToTeam = false;
        protected InventorySellDescriptor sellDescriptor;

		public VirtualItemType VirtualItemType
		{
			get
			{
				if (!string.IsNullOrEmpty(type) && type.Equals("virtual_currency"))
					return VirtualItemType.VirtualCurrency;

                if (attributes != null)
                {
                    for (int i = 0; i < attributes.Length; i++)
                    {
                        if (attributes[i].external_id.StartsWith(Constants.ATTRIBUTE_GOAL_ITEM))
                        {
                            return VirtualItemType.Hint;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(virtual_item_type))
                {
                    switch (virtual_item_type)
                    {
                        case "consumable": return VirtualItemType.Consumable;
                        case "non_consumable": return VirtualItemType.NonConsumable;
                        case "non_renewing_subscription": return VirtualItemType.NonRenewingSubscription;
                        case "hint": return VirtualItemType.Hint;
                    }
                }

                return VirtualItemType.None;
            }
		}

        public int GoalID
        {
            get
            {
                if(VirtualItemType != VirtualItemType.Hint)
                {
                    return -1;
                }
                for (int i = 0; i < attributes.Length; i++)
                {
                    if (attributes[i].external_id.StartsWith(Constants.ATTRIBUTE_GOAL_ITEM) && !attributes[i].external_id.StartsWith(Constants.ATTRIBUTE_GOAL_ITEM_ORDER))
                    {
                        return int.Parse(attributes[i].values[0].external_id.Replace(Constants.GOAL_ITEM_PREFIX, ""));
                    }
                }
                XDebug.LogError($"NO GOAL ID PRESENT! {sku}.");
                return -1;
            }
        }

        public int GoalOrder
        {
            get
            {
                if (VirtualItemType != VirtualItemType.Hint)
                {
                    return -1;
                }
                for (int i = 0; i < attributes.Length; i++)
                {
                    if (attributes[i].external_id.StartsWith(Constants.ATTRIBUTE_GOAL_ITEM_ORDER))
                    {
                        return int.Parse(attributes[i].values[0].external_id.Replace(Constants.GOAL_ITEM_ORDER_PREFIX, ""));
                    }
                }
                XDebug.LogError($"NO GOAL ORDER PRESENT! {sku}.");
                return -1;
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
                sellDescriptor.currencySku = vcurrencySku;
                sellDescriptor.currencyName = Constants.VCURENCY_SKU_TO_NAME[vcurrencySku];
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

        public bool MergeFrom(InventoryItem incoming)
        {
            if (this.sku != incoming.sku)
            {
                throw new ArgumentException($"Items are not of the same SKU: {incoming.sku} is not {this.sku}");
            }
            bool changed = false;
            if (!string.IsNullOrEmpty(incoming.name) && this.name != incoming.name)
            {
                this.name = incoming.name;
                changed = true;
            }
            if (!string.IsNullOrEmpty(incoming.description) && incoming.description != this.description)
            {
                if (this.description != "")
                {
                    this.description = incoming.description + this.description;
                }
                else
                {
                    this.description = incoming.description;
                }
                changed = true;
            }
            if (!string.IsNullOrEmpty(incoming.type) && this.type != incoming.type)
            {
                this.type = incoming.type;
                changed = true;
            }
            if (!string.IsNullOrEmpty(incoming.virtual_item_type) && this.virtual_item_type != incoming.virtual_item_type)
            {
                this.virtual_item_type = incoming.virtual_item_type;
                changed = true;
            }
            if (!string.IsNullOrEmpty(incoming.image_url) && incoming.image_url != this.image_url)
            {
                this.image_url = incoming.image_url;
            }
            if (incoming.quantity > 0 && incoming.quantity != this.quantity)
            {
                this.quantity = incoming.quantity;
                changed = true;
            }
            if (incoming.remaining_uses != null && this.remaining_uses != incoming.remaining_uses)
            {
                this.remaining_uses = incoming.remaining_uses;
                changed = true;
            }
            if (!string.IsNullOrEmpty(incoming.instance_id) && this.instance_id == incoming.instance_id)
            {
                this.instance_id = incoming.instance_id;
                changed = true;
            }
            if (incoming.attributes != null)
            {
                List<StoreItemAttribute> atts = new List<StoreItemAttribute>();
                foreach (StoreItemAttribute i in incoming.attributes)
                {
                    bool aFound = false;
                    if (this.attributes != null)
                    {
                        foreach (StoreItemAttribute j in this.attributes)
                        {
                            if (i.name == j.name)
                            {
                                aFound = true;
                                foreach (StoreItemAttribute.ValuePair p in i.values)
                                {
                                    bool valFound = false;
                                    foreach (StoreItemAttribute.ValuePair q in j.values)
                                    {
                                        if (p.value == q.value)
                                        {
                                            valFound = true;
                                        }
                                    }
                                    if (!valFound)
                                    {
                                        aFound = false;
                                    }
                                }
                            }
                        }
                    }
                    if (!aFound)
                    {
                        changed = true;
                        atts.Add(i);
                    }
                }
                if (atts.Count > 0)
                {
                    this.attributes = atts.ToArray();
                }
            }
            if (incoming.groups != null)
            {
                List<StoreItemGroup> grps = new List<StoreItemGroup>();
                foreach (StoreItemGroup i in incoming.groups)
                {
                    bool aFound = false;
                    if (this.groups != null)
                    {
                        foreach (StoreItemGroup j in this.groups)
                        {
                            if (i.name == j.name && i.external_id == j.external_id)
                            {
                                aFound = true;
                            }
                        }
                    }
                    if (!aFound)
                    {
                        grps.Add(i);
                        changed = true;
                    }
                }
                if (grps.Count > 0)
                {
                    this.groups = grps.ToArray();
                }
            }
            return changed;
        }
    }

    public class InventorySellDescriptor
    {
        public InventoryItem item;
        public bool isSellable = false;
        public string currencySku = Constants.RIS_CURRENCY_SKU;
        public string currencyName = Constants.RIS_CURRENCY_NAME;
        public int exchangeRate = 0;

        private Price price;

        public Price Price
        {
            get
            {
                if (price == null) {
                    if(item.sku == Constants.RIS_CURRENCY_SKU)
                    {
                        exchangeRate = 1;
                    }
                    price = new Price();
                    price.amount = (exchangeRate).ToString();
                    price.currency = currencySku;
                    price.amount_without_discount = (exchangeRate).ToString();
                }
                return price;
            }
        }
    }

    [Serializable]
    public class AuthInventoryItemEvent : UnityEvent<User>
    {

    }
}