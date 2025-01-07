using System;

namespace RabbitWings.Auth
{
	[Serializable]
	internal class AuthWithSocialNetworkAccessTokenRequest
	{
		public string access_token;
		public string access_token_secret;
		public string openId;
	}
}