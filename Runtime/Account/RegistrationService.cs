using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RabbitWings.Core;
using RabbitWings.Account;
using RabbitWings.Core;
using Newtonsoft.Json;

namespace RabbitWings.Account
{
    public static class RegistrationService
    {
        public static async Task RegisterAsync(
            string firstName,
            string lastName,
            string email,
            string password,
            string gender,
            string nickname,
            string picture,
            string phone,
            string phoneAuth,
            string birthday,
            string country,
            Action<string> onSuccess,
            Action<Error> onError)
        {
            try
            {
                DataValidator.ValidateRegisterData(email, phone, password);

                var requestData = new RegisterRequest
                {
                    Birthday = birthday,
                    Country = country,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    Password = password,
                    Gender = gender,
                    Nickname = nickname,
                    Picture = picture,
                    Phone = phone,
                    PhoneAuth = phoneAuth
                };

                string url = "https://kithfusioncache.azurewebsites.net/api/registerHandler";
                string jsonBody = JsonConvert.SerializeObject(requestData);
                Logger.LogMessage("Registration Request Payload:\n" + jsonBody);

                using (HttpClient client = new())
                {
                    ApiKeyHandler.AddApiKeyHeader(client);

                    var response = await client.PostAsync(url, new StringContent(jsonBody, Encoding.UTF8, "application/json"));
                    string responseContent = await response.Content.ReadAsStringAsync();

                    Logger.LogMessage($"Registration Response:\nStatusCode: {response.StatusCode}\nResponse Body: {responseContent}");

                    if (response.IsSuccessStatusCode)
                    {
                        onSuccess?.Invoke("Registration successful: " + responseContent);
                    }
                    else
                    {
                        onError?.Invoke(new Error(
                            ErrorType.ServerError,
                            response.StatusCode.ToString(),
                            "REGISTRATION_FAILED",
                            "Request failed: " + responseContent
                        ));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogMessage($"Registration Exception:\n{ex.Message}");
                onError?.Invoke(new Error(
                    ErrorType.UnknownError,
                    "Unknown",
                    "UNEXPECTED_ERROR",
                    $"An error occurred: {ex.Message}"
                ));
            }
        }
    }
}