using Newtonsoft.Json;
using RabbitWings.Inventory;
using System;

namespace RabbitWings.Core
{
	[Serializable]
	public class StoreItem : PurchasableItem
    {
		public string virtual_item_type;
		public string long_description;
		public InventoryOptions inventory_options;
		public int order;
		public MediaListItem[] media_list;

		private InventoryItem invItm;

		[JsonIgnore]
		public InventoryItem InventoryItem
		{
			get
			{
				if (invItm == null) {
					invItm = new InventoryItem();
					invItm.type = type;
					invItm.groups = groups;
					invItm.attributes = attributes;
					invItm.sku = sku;
					invItm.name = name;
					invItm.description = description;
					invItm.image_url = image_url;
					invItm.remaining_uses = 0;
					invItm.virtual_item_type = virtual_item_type;
				}
				return invItm;
			}
		}

		public VirtualItemType VirtualItemType
		{
			get
			{
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

		[Serializable]
		public class InventoryOptions
		{
			public ConsumableOption consumable;
			public ExpirationPeriod expiration_period;

			[Serializable]
			public class ConsumableOption
			{
				public int? usages_count;
			}

			[Serializable]
			public class ExpirationPeriod
			{
				public string type;
				public int value;

				public TimeSpan ToTimeSpan()
				{
					var dt = DateTime.Now;
					switch (type)
					{
						case "minute":
							dt = dt.AddMinutes(value);
							break;
						case "hour":
							dt = dt.AddHours(value);
							break;
						case "day":
							dt = dt.AddDays(value);
							break;
						case "week":
							dt = dt.AddDays(7 * value);
							break;
						case "month":
							dt = dt.AddMonths(value);
							break;
						case "year":
							dt = dt.AddYears(value);
							break;
					}

					return dt - DateTime.Now;
				}

				public override string ToString()
				{
					var result = $"{value} {type}";
					if (value > 1)
						result += "s";
					return result;
				}
			}
		}
	}
}