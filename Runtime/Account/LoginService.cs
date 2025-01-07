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
    public static class LoginService
    {
        public static async Task LoginAsync(
            string email,
            string password,
            Action<string> onSuccess,
            Action<Error> onError)
        {
            try
            {
                DataValidator.ValidateLoginData(email, password);

                var loginRequest = new LoginRequest
                {
                    Email = email,
                    Password = password
                };

                Console.WriteLine("Login Payload:");
                Console.WriteLine(JsonConvert.SerializeObject(loginRequest, Formatting.Indented));

                string jsonBody = JsonConvert.SerializeObject(loginRequest);
                Logger.LogMessage("Login Payload:\n" + jsonBody);

                string url = "https://kithfusioncache.azurewebsites.net/api/loginHandler";

                using (HttpClient client = new HttpClient())
                {
                    ApiKeyHandler.AddApiKeyHeader(client);

                    var response = await client.PostAsync(url, new StringContent(jsonBody, Encoding.UTF8, "application/json"));

                    string stringResponse = await response.Content.ReadAsStringAsync();
                    Logger.LogMessage("Login Response:\n" + stringResponse);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        onSuccess?.Invoke("Login successful: " + responseContent);
                    }
                    else
                    {
                        string errorContent = await response.Content.ReadAsStringAsync();
                        onError?.Invoke(new Error(
                            ErrorType.RequestError,
                            response.StatusCode.ToString(),
                            "LOGIN_FAILED",
                            $"Login failed: {errorContent}"
                        ));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogMessage("Login Exception:\n" + ex.Message);
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