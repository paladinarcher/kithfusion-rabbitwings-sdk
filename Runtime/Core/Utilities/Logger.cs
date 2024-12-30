using UnityEngine;

namespace RabbitWings.Core.Utilities
{
	/// <summary>
	/// Provides centralized logging functionality for the RabbitWings SDK.
	/// Logs messages to the appropriate logging system based on the environment.
	/// </summary>
	public static class Logger
	{
		/// <summary>
		/// Logs a message to the appropriate logging system (Unity or Console).
		/// </summary>
		/// <param name="message">The message to log.</param>
		public static void LogMessage(string message)
		{

            Debug.Log(message); 

		}
	}
}
