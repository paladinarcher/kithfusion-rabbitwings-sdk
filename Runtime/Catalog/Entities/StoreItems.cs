using System;
using RabbitWings.Core;

namespace RabbitWings.Catalog
{
	[Serializable]
	public class StoreItems
	{
		public bool has_more;
		public StoreItem[] items;
	}
}