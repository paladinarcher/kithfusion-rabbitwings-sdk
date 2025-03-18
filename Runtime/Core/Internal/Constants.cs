using System.Collections.Generic;
using System.Linq;

namespace RabbitWings.Core
{
	public static class Constants
	{
		public static string SDK_VERSION
		{
			get
			{
				return sdkVersion;
			}
		}
		private static string sdkVersion = "1.1.26";

		public const string DEFAULT_PROJECT_ID = "77640";
		public const string DEFAULT_LOGIN_ID = "026201e3-7e40-11ea-a85b-42010aa80004";
		public const int DEFAULT_OAUTH_CLIENT_ID = 57;
		public const string DEFAULT_REDIRECT_URL = "https://login.xsolla.com/api/blank";
		public const string DEFAULT_WEB_STORE_URL = "https://sitebuilder.xsolla.com/game/sdk-web-store/";

		public const string PAYSTATION_URL = "https://secure.xsolla.com/paystation3/";
		public const string PAYSTATION_SANDBOX_URL = "https://sandbox-secure.xsolla.com/paystation3/";

		public const string WEB_BROWSER_RESOURCE_PATH = "XsollaWebBrowser";
		public const string BROWSER_REVISION = "1069273";
		public const string CUSTOM_BROWSER_USER_AGENT = null;

		public const float WEB_SOCKETS_TIMEOUT = 300f;
		public const float SHORT_POLLING_INTERVAL = 3f;
		public const float SHORT_POLLING_LIMIT = 600f;

        public const string ITEM_GROUP_NOT_FOR_SALE = "nosell";
        public const string ATTRIBUTE_INVENTORY_SHOW = "inventory_show";
        public const string ATTRIBUTE_GOAL_ITEM = "goal_item";
        public const string ATTRIBUTE_PHYSICAL_ITEM = "physical_item";
        public const string ATTRIBUTE_GOAL_ITEM_ORDER = "goal_item_order";
        public const string ATTRIBUTE_INVENTORY_SHOW_VALUE = "yes";
        public const string CURRENCY_VALUE_ATTRIBUTE_ID = "ris_credit_sell_value";
        public const string RIS_CURRENCY_SKU = "ris-credits";
		public const string RIS_CURRENCY_NAME = "RIS Credits";
        public const string RIS_CURRENCY_VALUE_PREFIX = "ris_";
        public const string GOAL_ITEM_PREFIX = "goal_";
		public const string GOAL_ITEM_ORDER_PREFIX = "goal_order_";
        public const string DEFAULT_DEBUG_PASSWORD = "asdf1234";
        public const string VCURRENCY_PACKAGE_SKU = "RISCurrency";
		private static Dictionary<string, string> VCURENCY_SKU_TO_NAME_P;

        public static Dictionary<string, string> VCURENCY_SKU_TO_NAME { 
			get
			{
				if (VCURENCY_SKU_TO_NAME_P == null)
				{
					VCURENCY_SKU_TO_NAME_P = new Dictionary<string, string>
					{
						{ RIS_CURRENCY_SKU, RIS_CURRENCY_NAME }
					};
				}
				return VCURENCY_SKU_TO_NAME_P;
			}
		}
    }
}