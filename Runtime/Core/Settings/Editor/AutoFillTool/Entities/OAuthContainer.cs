using System;

namespace RabbitWings.Core.Editor.AutoFillSettings
{
	[Serializable]
	public class OAuthContainer
	{
		public int id;
		public bool is_public;
		public string name;
		public string[] redirect_uris;
	}
}