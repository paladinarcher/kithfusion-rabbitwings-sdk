﻿using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RabbitWings.Core.Entities;
using RabbitWings.Core.Utilities;
using RabbitWings.Catalog.Entities;
using Newtonsoft.Json;

namespace RabbitWings.Catalog
{
    /// <summary>
    /// Provides catalog-related services, such as fetching catalogs or specific items.
    /// </summary>
    public static class CatalogService
    {
        /// <summary>
        /// Fetches all catalogs from the API.
        /// </summary>
        /// <param name="onSuccess">Callback to handle a successful response.</param>
        /// <param name="onError">Callback to handle errors with a <see cref="CustomError"/>.</param>
        public static async Task GetAllCatalogs(
            Action<string> onSuccess,
            Action<CustomError> onError)
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

        /// <summary>
        /// Fetches a specific item from the catalog based on the provided parameters.
        /// </summary>
        /// <param name="id">The item ID.</param>
        /// <param name="type">The item type.</param>
        /// <param name="sku">The item SKU (nullable).</param>
        /// <param name="name">The item name.</param>
        /// <param name="onSuccess">Callback to handle a successful response.</param>
        /// <param name="onError">Callback to handle errors with a <see cref="CustomError"/>.</param>
        public static async Task GetSpecificItem(
            string id,
            string type,
            int? sku,
            string name,
            Action<string> onSuccess,
            Action<CustomError> onError)
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

        /// <summary>
        /// Sends a generic catalog request to the specified endpoint.
        /// </summary>
        private static async Task SendCatalogRequest(
            string endpoint,
            string id,
            string type,
            int? sku,
            string name,
            Action<string> onSuccess,
            Action<CustomError> onError,
            HttpMethod method)
        {
            try
            {
                string url = $"https://kithfusioncache.azurewebsites.net/api/{endpoint}";

                // Log the request URL.
                Logger.LogMessage("Request URL: " + url);

                using (HttpClient client = new HttpClient())
                {
                    // Use ApiKeyHandler to add the API key to the request headers
                    ApiKeyHandler.AddApiKeyHeader(client);

                    HttpRequestMessage requestMessage = new HttpRequestMessage(method, url);

                    if (method == HttpMethod.Post)
                    {
                        var requestData = new GetItemRequest
                        {
                            id = id,
                            type = type,
                            sku = sku ?? 0, // Default to 0 if sku is null
                            name = name
                        };

                        // Serialize the request data to JSON.
                        string jsonContent = JsonConvert.SerializeObject(requestData);

                        // Log the raw request payload.
                        Logger.LogMessage("Catalog Request Payload:\n" + jsonContent);

                        requestMessage.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    }

                    // Send the HTTP request.
                    var response = await client.SendAsync(requestMessage);

                    // Extract and log the response content.
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Logger.LogMessage($"Catalog Response:\nStatusCode: {response.StatusCode}\nResponse Body: {responseContent}");

                    if (response.IsSuccessStatusCode)
                    {
                        // Invoke the success callback with the response content.
                        onSuccess?.Invoke(responseContent);
                    }
                    else
                    {
                        // Invoke the error callback with detailed error information.
                        onError?.Invoke(new CustomError
                        {
                            StatusCode = response.StatusCode.ToString(),
                            ErrorMessage = responseContent
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception details.
                Logger.LogMessage($"Catalog Request Exception:\n{ex.Message}");

                // Invoke the error callback with the exception details.
                onError?.Invoke(new CustomError
                {
                    StatusCode = "Unknown",
                    ErrorMessage = ex.Message
                });
            }
        }
    }
}
