using System;
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
	public class BundleItem : PurchasableItem
    {
		public string bundle_type;
		public bool can_be_bought;
		public Price total_content_price;
		public Content[] content;

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