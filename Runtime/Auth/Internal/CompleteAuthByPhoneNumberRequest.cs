using System;

namespace RabbitWings.Auth
{
	[Serializable]
	internal class CompleteAuthByPhoneNumberRequest
	{
		public string phone_number;
		public string code;
		public string operation_id;
	}
}