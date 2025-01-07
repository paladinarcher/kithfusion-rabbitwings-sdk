using System;
using RabbitWings.Core;

namespace RabbitWings.Auth
{
	internal class StandaloneSocialAuth
	{
		public void Perform(SocialProvider provider, Action onSuccess, Action<Error> onError, Action onCancel)
		{
			var socialNetworkAuthUrl = Auth.GetSocialNetworkAuthUrl(provider);
			var browser = XsollaWebBrowser.InAppBrowser;
			browser.Open(socialNetworkAuthUrl);

			browser.AddCloseHandler(() => onCancel?.Invoke());
			browser.AddUrlChangeHandler(url => UrlChangedHandler(url, onSuccess, onError));
		}

		private static void UrlChangedHandler(string url, Action onSuccess, Action<Error> onError)
		{
			if (ParseUtils.TryGetValueFromUrl(url, ParseParameter.code, out var code))
			{
				Auth.ExchangeCodeToToken(
					code,
					() =>
					{
						XsollaWebBrowser.Close();
						onSuccess?.Invoke();
					},
					onError);
			}
		}
	}
}