using System;

namespace RabbitWings.UserAccount
{
	[Serializable]
	public class LinkDeviceRequest
	{
		public string device;
		public string device_id;
	}
}