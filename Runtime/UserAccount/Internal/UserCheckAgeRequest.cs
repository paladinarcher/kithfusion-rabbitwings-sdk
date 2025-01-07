using System;

namespace RabbitWings.UserAccount
{
	[Serializable]
	internal class UserCheckAgeRequest
	{
		public string dob;
		public string project_id;
	}
}