using System;

namespace RabbitWings.UserAccount
{
	[Serializable]
	public class UserInfoUpdate
	{
		public string birthday;
		public string first_name;
		public string gender;
		public string last_name;
		public string nickname;
	}
}