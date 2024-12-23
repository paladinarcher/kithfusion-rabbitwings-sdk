using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RabbitWings.Orders.Entities;
using RabbitWings.Core.Entities;
using RabbitWings.Core.Utilities;

namespace RabbitWings.Orders
{
    public static class OrderService
    {
        private const string BaseUrl = "https://kithfusioncache.azurewebsites.net/api";

        /// <summary>
        /// Creates a new transaction by sending a POST request.
        /// </summary>
        /// <param name="request">Transaction request object containing transaction details.</param>
        /// <param name="onSuccess">Action to execute on a successful response.</param>
        /// <param name="onError">Action to execute on an error response.</param>
        public static async Task CreateTransaction(
            TransactionRequest request,
            Action<string> onSuccess,
            Action<CustomError> onError)
        {
            string endpoint = "/transactionHandler";
            await SendTransactionRequest(endpoint, request, onSuccess, onError);
        }

        /// <summary>
        /// Sends the transaction request to the specified endpoint.
        /// </summary>
        /// <param name="endpoint">API endpoint for the transaction.</param>
        /// <param name="request">Request payload for the transaction.</param>
        /// <param name="onSuccess">Action to execute on a successful response.</param>
        /// <param name="onError">Action to execute on an error response.</param>
        private static async Task SendTransactionRequest(
            string endpoint,
            TransactionRequest request,
            Action<string> onSuccess,
            Action<CustomError> onError)
        {
            try
            {
                string url = $"{BaseUrl}{endpoint}";
                Console.WriteLine($"Request URL: {url}");

                using (HttpClient client = new HttpClient())
                {
                    // Add API key using centralized handler
                    ApiKeyHandler.AddApiKeyHeader(client);

                    // Serialize the request object to JSON
                    string jsonContent = JsonConvert.SerializeObject(request);
                    var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    // Send the POST request
                    var response = await client.PostAsync(url, httpContent);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        onSuccess?.Invoke(responseContent);
                    }
                    else
                    {
                        string errorContent = await response.Content.ReadAsStringAsync();
                        onError?.Invoke(new CustomError
                        {
                            StatusCode = response.StatusCode.ToString(),
                            ErrorMessage = errorContent
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                onError?.Invoke(new CustomError
                {
                    StatusCode = "Unknown",
                    ErrorMessage = ex.Message
                });
            }
        }
    }
}
