using System;

namespace RabbitWings.Auth
{
	[Serializable]
	internal class SignInRequest
	{
		public string username;
		public string password;
		public bool? remember_me;
	}
}