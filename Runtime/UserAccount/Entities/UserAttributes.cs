using System;
using System.Collections.Generic;

namespace RabbitWings.UserAccount
{
	[Serializable]
	public class UserAttributes
	{
		public List<UserAttribute> items;
	}
	
	[Serializable]
	public class UserAttribute
	{
		public string key;
		public string permission;
		public string value;
	}
}