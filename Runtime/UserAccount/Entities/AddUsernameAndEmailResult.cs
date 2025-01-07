using System;

namespace RabbitWings.UserAccount
{
	[Serializable]
	public class AddUsernameAndEmailResult
	{
		public bool email_confirmation_required;
	}
}