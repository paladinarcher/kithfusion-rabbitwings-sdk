using System;
using RabbitWings.Core;

namespace RabbitWings.Catalog
{
	[Serializable]
	public class StoreShortItems
	{
		public StoreShortItem[] items;
	}

	[Serializable]
	public class StoreShortItem
	{
		public string sku;
		public string name;
		public string description;
		public StoreItemGroup[] groups;
		public StoreItemPromotion[] promotions;
	}
}