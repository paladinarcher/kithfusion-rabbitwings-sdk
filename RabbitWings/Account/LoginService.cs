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
    /// Provides functionality for user authentication, including login.
    /// </summary>
    public static class LoginService
    {
        /// <summary>
        /// Handles the user login process by validating input, creating a login request,
        /// and sending the data to the login API endpoint.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="password">The user's password.</param>
        /// <param name="onSuccess">Callback to invoke on successful login with a message.</param>
        /// <param name="onError">Callback to invoke on login failure with a <see cref="CustomError"/>.</param>
        public static async Task LoginAsync(
            string email,
            string password,
            Action<string> onSuccess,
            Action<CustomError> onError)
        {
            try
            {
                // Validate login credentials using core functionality.
                DataValidator.ValidateLoginData(email, password);

                // Construct the login request object.
                var loginRequest = new LoginRequest
                {
                    Email = email,
                    Password = password
                };

                // Log the serialized login request for debugging (can be removed for production).
                Console.WriteLine("Login Payload:");
                Console.WriteLine(JsonConvert.SerializeObject(loginRequest, Formatting.Indented));

                // Serialize the login request to JSON format.
                string jsonBody = JsonConvert.SerializeObject(loginRequest);

                // Define the login API endpoint.
                string url = "https://kithfusioncache.azurewebsites.net/api/loginHandler";

                // Perform the API request.
                using (HttpClient client = new HttpClient())
                {
                    // Use ApiKeyHandler from Core to add the API key to the headers.
                    ApiKeyHandler.AddApiKeyHeader(client);

                    // Send the HTTP POST request with the login JSON payload.
                    var response = await client.PostAsync(url, new StringContent(jsonBody, Encoding.UTF8, "application/json"));

                    // Handle the response based on its status.
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content.
                        string responseContent = await response.Content.ReadAsStringAsync();

                        // Invoke the success callback with the response message.
                        onSuccess?.Invoke("Login successful: " + responseContent);
                    }
                    else
                    {
                        // Extract and log the error details.
                        string errorContent = await response.Content.ReadAsStringAsync();

                        // Invoke the error callback with a detailed error object.
                        onError?.Invoke(new CustomError
                        {
                            StatusCode = response.StatusCode.ToString(),
                            ErrorMessage = $"Login failed: {errorContent}"
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle unexpected exceptions and invoke the error callback.
                onError?.Invoke(new CustomError
                {
                    StatusCode = "Unknown",
                    ErrorMessage = $"An error occurred: {ex.Message}"
                });
            }
        }
    }
}
