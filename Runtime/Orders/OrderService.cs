using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RabbitWings.Orders;
using RabbitWings.Core;
using RabbitWings.Core;

namespace RabbitWings.Orders
{
    public static class OrderService
    {
        private const string BaseUrl = "https://kithfusioncache.azurewebsites.net/api";

        public static async Task CreateTransaction(
            TransactionRequest request,
            Action<string> onSuccess,
            Action<Error> onError)
        {
            string endpoint = "/transactionHandler";
            await SendTransactionRequest(endpoint, request, onSuccess, onError);
        }

        private static async Task SendTransactionRequest(
            string endpoint,
            TransactionRequest request,
            Action<string> onSuccess,
            Action<Error> onError)
        {
            try
            {
                string url = $"{BaseUrl}{endpoint}";
                Logger.LogMessage($"Transaction Request URL: {url}");

                using (HttpClient client = new HttpClient())
                {
                    ApiKeyHandler.AddApiKeyHeader(client);
                    string jsonContent = JsonConvert.SerializeObject(request);
                    Logger.LogMessage("Transaction Request Payload:\n" + jsonContent);

                    var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(url, httpContent);

                    string responseContent = await response.Content.ReadAsStringAsync();
                    Logger.LogMessage($"Transaction Response:\nStatusCode: {response.StatusCode}\nResponse Body: {responseContent}");

                    if (response.IsSuccessStatusCode)
                    {
                        onSuccess?.Invoke(responseContent);
                    }
                    else
                    {
                        onError?.Invoke(new Error(
                            ErrorType.RequestError,
                            response.StatusCode.ToString(),
                            "TRANSACTION_FAILED",
                            responseContent
                        ));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogMessage($"Transaction Request Exception:\n{ex.Message}");
                onError?.Invoke(new Error(
                    ErrorType.UnknownError,
                    "Unknown",
                    "TRANSACTION_UNEXPECTED_ERROR",
                    ex.Message
                ));
            }
        }
    }
}