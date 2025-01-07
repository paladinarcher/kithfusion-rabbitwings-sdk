using System;
using RabbitWings.Core;

namespace RabbitWings.Catalog
{
	[Serializable]
	public class VirtualCurrencyItems
	{
		public VirtualCurrencyItem[] items;
	}

	[Serializable]
	public class VirtualCurrencyItem : StoreItem { }
}