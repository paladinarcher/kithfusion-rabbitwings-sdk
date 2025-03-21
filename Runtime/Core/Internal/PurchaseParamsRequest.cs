using System;
using System.Collections.Generic;

namespace RabbitWings.Core
{
	[Serializable]
	internal class PurchaseParamsRequest
	{
		public bool sandbox;
		public Settings settings;
		public Dictionary<string, object> custom_parameters;
		public string currency;
		public string locale;
		public int? quantity;
		public ShippingData shipping_data;
		public Dictionary<string, object> shipping_method;

		[Serializable]
		public class Settings
		{
			public string external_id;
			public string return_url;
			public PayStationUI ui;
			public RedirectPolicy redirect_policy;
		}
	}
}