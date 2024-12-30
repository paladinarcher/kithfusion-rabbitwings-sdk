using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RabbitWings.Core.Utilities;
using RabbitWings.Account.Entities;
using RabbitWings.Core.Entities;
using Newtonsoft.Json;

namespace RabbitWings.Account
{
    /// <summary>
    /// Provides functionality for user registration, including creating a new account.
    /// </summary>
    public static class RegistrationService
    {
        /// <summary>
        /// Registers a new user with the provided details.
        /// </summary>
        /// <param name="firstName">The user's first name.</param>
        /// <param name="lastName">The user's last name.</param>
        /// <param name="email">The user's email address.</param>
        /// <param name="password">The user's password.</param>
        /// <param name="gender">The user's gender.</param>
        /// <param name="nickname">The user's nickname.</param>
        /// <param name="picture">The user's profile picture URL.</param>
        /// <param name="phone">The user's phone number.</param>
        /// <param name="phoneAuth">The user's phone authentication method.</param>
        /// <param name="birthday">The user's birth date.</param>
        /// <param name="country">The user's country.</param>
        /// <param name="onSuccess">Callback invoked upon successful registration.</param>
        /// <param name="onError">Callback invoked on registration failure.</param>
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
            Action<CustomError> onError)
        {
            try
            {
                // Validate the registration data using the core DataValidator.
                DataValidator.ValidateRegisterData(email, phone, password);

                // Create the registration request data object.
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

                // URL for the registration request.
                string url = "https://kithfusioncache.azurewebsites.net/api/registerHandler";

                // Serialize the request data to JSON format.
                string jsonBody = JsonConvert.SerializeObject(requestData);

                // Log the raw request payload.
                Logger.LogMessage("Registration Request Payload:\n" + jsonBody);

                using (HttpClient client = new())
                {
                    // Use ApiKeyHandler from Core to add the API key to the request headers.
                    ApiKeyHandler.AddApiKeyHeader(client);

                    // Send the HTTP POST request with the serialized JSON content.
                    var response = await client.PostAsync(url, new StringContent(jsonBody, Encoding.UTF8, "application/json"));

                    // Extract the response content.
                    string responseContent = await response.Content.ReadAsStringAsync();

                    // Log the response details.
                    Logger.LogMessage($"Registration Response:\nStatusCode: {response.StatusCode}\nResponse Body: {responseContent}");

                    // Handle the response based on its status.
                    if (response.IsSuccessStatusCode)
                    {
                        // Invoke the success callback with the response message.
                        onSuccess?.Invoke("Registration successful: " + responseContent);
                    }
                    else
                    {
                        // Invoke the error callback with a detailed error object.
                        onError?.Invoke(new CustomError
                        {
                            StatusCode = response.StatusCode.ToString(),
                            ErrorMessage = "Request failed: " + responseContent
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception details.
                Logger.LogMessage($"Registration Exception:\n{ex.Message}");

                // Invoke the error callback with the exception details.
                onError?.Invoke(new CustomError
                {
                    StatusCode = "Unknown",
                    ErrorMessage = $"An error occurred: {ex.Message}"
                });
            }
        }
    }
}
