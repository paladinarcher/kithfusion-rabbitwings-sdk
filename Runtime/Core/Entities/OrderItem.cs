using System;

namespace RabbitWings.Core
{
	[Serializable]
	public class OrderItem
	{
		public string sku;
		public int quantity;
		public string is_free; // yes or anything else
		public Price price;
	}
}