using System;

namespace RabbitWings.Inventory
{
	[Serializable]
	public class ConsumeItem
	{
		public string sku;
		public int? quantity;
		public string instance_id;
	}
}