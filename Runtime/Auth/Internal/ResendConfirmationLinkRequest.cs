using System;

namespace RabbitWings.Auth
{
	[Serializable]
	internal class ResendConfirmationLinkRequest
	{
		public string username;
	}
}