﻿using System;
using RabbitWings.Core;

namespace RabbitWings.Catalog
{
	[Serializable]
	public class BundleItems
	{
		public bool has_more;
		public BundleItem[] items;
	}

	[Serializable]
	public class BundleItem
	{
		public string sku;
		public string name;
		public StoreItemGroup[] groups;
		public string description;
		public StoreItemAttribute[] attributes;
		public string type;
		public string bundle_type;
		public string image_url;
		public bool is_free;
		public bool can_be_bought;
		public Price price;
		public Price total_content_price;
		public VirtualPrice[] virtual_prices;
		public Content[] content;
		public StoreItemPromotion[] promotions;
		public StoreItemLimits limits;

		[Serializable]
		public class Content
		{
			public string sku;
			public string name;
			public string type;
			public string description;
			public string image_url;
			public int quantity;
			public Price price;
			public VirtualPrice[] virtual_prices;
		}
	}
}