using System;
using System.Net.Http;

namespace RabbitWings.Core
{
    /// <summary>
    /// Handles API key management and validation.
    /// </summary>
    public static class ApiKeyHandler
    {
        // Securely retrieve the API key (e.g., from environment variables or configuration)
        private static readonly string ApiKey = Environment.GetEnvironmentVariable("API_KEY") ?? throw new InvalidOperationException("API Key not found");

        /// <summary>
        /// Adds the API key to the HTTP client headers.
        /// </summary>
        /// <param name="client">The HTTP client to add the API key to.</param>
        public static void AddApiKeyHeader(HttpClient client)
        {
            if (string.IsNullOrWhiteSpace(ApiKey))
            {
                throw new InvalidOperationException("Invalid API Key");
            }

            client.DefaultRequestHeaders.Add("x-api-key", ApiKey);
        }
    }
}
