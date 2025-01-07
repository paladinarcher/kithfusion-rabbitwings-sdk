using System;

namespace RabbitWings.Auth
{
	[Serializable]
	internal class StartAuthByEmailRequest
	{
		public string email;
		public string link_url;
		public bool? send_link;
	}
}