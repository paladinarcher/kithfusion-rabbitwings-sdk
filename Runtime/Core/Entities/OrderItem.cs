using System;

namespace RabbitWings.Core
{
	[Serializable]
	public class OrderItem
	{
		public string sku;
		public int quantity;
		public string is_free;
		public Price price;
	}
}