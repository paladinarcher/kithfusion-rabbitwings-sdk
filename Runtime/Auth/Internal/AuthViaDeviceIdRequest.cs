using System;

namespace RabbitWings.Auth
{
	[Serializable]
	internal class AuthViaDeviceIdRequest
	{
		public string device;
		public string device_id;
	}
}