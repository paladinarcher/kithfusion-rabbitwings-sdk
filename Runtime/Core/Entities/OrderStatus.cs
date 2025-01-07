using System;

namespace RabbitWings.Core
{
	[Serializable]
	public class OrderStatus
	{
		public int order_id;
		public string status;
		public OrderContent content;
	}
}