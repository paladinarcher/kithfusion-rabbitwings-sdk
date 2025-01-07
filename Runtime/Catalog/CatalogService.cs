using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RabbitWings.Core;
using RabbitWings.Core;
using RabbitWings.Catalog;
using Newtonsoft.Json;

namespace RabbitWings.Catalog
{
    public static class CatalogService
    {
        public static async Task GetAllCatalogs(
            Action<string> onSuccess,
            Action<Error> onError)
        {
            await SendCatalogRequest(
                endpoint: "itemsHandler",
                id: null,
                type: null,
                sku: null,
                name: null,
                onSuccess: onSuccess,
                onError: onError,
                method: HttpMethod.Get);
        }

        public static async Task GetSpecificItem(
            string id,
            string type,
            int? sku,
            string name,
            Action<string> onSuccess,
            Action<Error> onError)
        {
            await SendCatalogRequest(
                endpoint: "itemsHandler",
                id: id,
                type: type,
                sku: sku,
                name: name,
                onSuccess: onSuccess,
                onError: onError,
                method: HttpMethod.Post);
        }

        private static async Task SendCatalogRequest(
            string endpoint,
            string id,
            string type,
            int? sku,
            string name,
            Action<string> onSuccess,
            Action<Error> onError,
            HttpMethod method)
        {
            try
            {
                string url = $"https://kithfusioncache.azurewebsites.net/api/{endpoint}";
                Logger.LogMessage("Request URL: " + url);

                using (HttpClient client = new HttpClient())
                {
                    ApiKeyHandler.AddApiKeyHeader(client);
                    HttpRequestMessage requestMessage = new HttpRequestMessage(method, url);

                    if (method == HttpMethod.Post)
                    {
                        var requestData = new GetItemRequest
                        {
                            id = id,
                            type = type,
                            sku = sku ?? 0,
                            name = name
                        };

                        string jsonContent = JsonConvert.SerializeObject(requestData);
                        Logger.LogMessage("Catalog Request Payload:\n" + jsonContent);
                        requestMessage.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    }

                    var response = await client.SendAsync(requestMessage);
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Logger.LogMessage($"Catalog Response:\nStatusCode: {response.StatusCode}\nResponse Body: {responseContent}");

                    if (response.IsSuccessStatusCode)
                    {
                        onSuccess?.Invoke(responseContent);
                    }
                    else
                    {
                        onError?.Invoke(new Error(
                            ErrorType.RequestError,
                            response.StatusCode.ToString(),
                            "CATALOG_REQUEST_FAILED",
                            responseContent
                        ));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogMessage($"Catalog Request Exception:\n{ex.Message}");
                onError?.Invoke(new Error(
                    ErrorType.UnknownError,
                    "Unknown",
                    "CATALOG_UNEXPECTED_ERROR",
                    ex.Message
                ));
            }
        }
    }
}