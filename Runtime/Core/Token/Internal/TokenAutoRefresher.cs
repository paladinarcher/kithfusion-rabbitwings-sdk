using System;
using RabbitWings.Auth;

namespace RabbitWings.Core
{
	internal static class TokenAutoRefresher
	{
		public static void Check(Error error, Action<Error> onError, Action onSuccess)
		{
			if (error.ErrorType != ErrorType.InvalidToken)
			{
				onError?.Invoke(error);
				return;
			}

		}
	}
}