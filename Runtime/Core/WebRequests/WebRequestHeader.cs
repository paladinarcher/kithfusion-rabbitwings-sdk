namespace RabbitWings.Core
{
	public class WebRequestHeader
	{
		public string Name { get; set; }

		public string Value { get; set; }

		public WebRequestHeader() { }

		public WebRequestHeader(string name, string value)
		{
			Name = name;
			Value = value;
		}

		public static WebRequestHeader AuthHeader()
		{
			var accessToken = Token.AccessToken;
			return !string.IsNullOrEmpty(accessToken)
				? AuthHeader(accessToken)
				: null;
		}

		public static WebRequestHeader AuthHeader(string token)
		{
			var header = new WebRequestHeader {
				Name = "Authorization",
				Value = $"Bearer {token}"
			};

			return header;
		}

		public static WebRequestHeader AuthXApi(string apiKey)
		{
			return new WebRequestHeader
			{
				Name = "x-api-key",
				Value = apiKey
			};
		}

        public static WebRequestHeader CurrentUser()
        {
			return AddUser(User.Current);
        }

        public static WebRequestHeader AddUser(User usr)
        {
            return new WebRequestHeader
            {
                Name = "x-user-id",
                Value = (usr != null && usr.id != null ? usr.id : "NO_CURRENT_USER")
            };
        }

        public static WebRequestHeader JsonContentTypeHeader()
		{
			return new WebRequestHeader {
				Name = "Content-Type",
				Value = "application/json"
			};
		}

		public static WebRequestHeader FormDataContentTypeHeader(string boundary)
		{
			return new WebRequestHeader {
				Name = "Content-type",
				Value = $"multipart/form-data; boundary ={boundary}"
			};
		}
	}
}