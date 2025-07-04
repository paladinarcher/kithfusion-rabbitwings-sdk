using System.Text;
using System.Web;

namespace RabbitWings.Core
{
	public class UrlBuilder
	{
		private readonly StringBuilder builder;
		private bool isFirstParamPassed;

		private string Separator
		{
			get
			{
				if (isFirstParamPassed)
					return "&";

				isFirstParamPassed = true;
				return "?";
			}
		}

		public UrlBuilder(string url)
		{
			builder = new StringBuilder(url);
			isFirstParamPassed = url.Contains("?");
		}

		public string Build()
		{
			return builder.ToString();
		}

		public UrlBuilder AddState(string value)
		{
			return AddParam("state", value);
		}

		public UrlBuilder AddRedirectUri(string value)
		{
			return AddParam("redirect_uri", value);
		}

		public UrlBuilder AddClientId(int value)
		{
			return AddParam("client_id", value.ToString());
		}

		public UrlBuilder AddProjectId(string value)
		{
			return AddParam("projectId", value);
		}

		public UrlBuilder AddResponseType(string value)
		{
			return AddParam("response_type", value);
		}

		public UrlBuilder AddScope(string value)
		{
			return AddParam("scope", value);
		}

		public UrlBuilder AddLimit(int? value)
		{
			return AddParam("limit", value?.ToString());
		}

		public UrlBuilder AddOffset(int? value)
		{
			return AddParam("offset", value?.ToString());
		}

		public UrlBuilder AddLocale(string value)
		{
			return AddParam("locale", value);
		}

		public UrlBuilder AddCurrency(string value)
		{
			return AddParam("currency", value);
		}

		public UrlBuilder AddPlatform(string value)
		{
			return AddParam("platform", value);
		}

		public UrlBuilder AddCountry(string value)
		{
			return AddParam("country", value);
		}

		public UrlBuilder AddAdditionalFields(string value)
		{
			return AddParamAsArray("additional_fields", value);
		}

		public UrlBuilder AddId(string value)
		{
			if(string.IsNullOrEmpty(value)) { return this; }
			return AddParam("id", value);
		}

		public UrlBuilder AddType(string value)
		{
			return AddParam("type", value);
		}

		public UrlBuilder AddMission(string value)
		{
			return AddParam("mission_name", value);
        }

        public UrlBuilder AddNetworkID(string value)
        {
            return AddParam("network_id", value);
        }

        public UrlBuilder AddArray<T>(string name, T[] values)
		{
			if (string.IsNullOrEmpty(name))
				return this;

			if (values == null || values.Length == 0)
				return this;

			foreach (var value in values)
			{
				AddParamAsArray(name, value?.ToString());
			}

			return this;
		}

		public UrlBuilder AddParam(string name, string value)
		{
			if (string.IsNullOrWhiteSpace(value))
				return this;

			builder.Append($"{Separator}{URLEncode(name)}={URLEncode(value)}");
			return this;
		}

		public UrlBuilder AddParam(string name, int value)
		{
			builder.Append($"{Separator}{URLEncode(name)}={value}");
			return this;
		}

		private UrlBuilder AddParamAsArray(string name, string value)
		{
			if (string.IsNullOrWhiteSpace(value))
				return this;

			builder.Append($"{Separator}{URLEncode(name)}[]={URLEncode(value)}");
			return this;
		}
		public string URLEncode(string val)
		{
			return HttpUtility.UrlEncode(val);
		}
	}
}