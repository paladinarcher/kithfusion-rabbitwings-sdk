using System;
using System.Threading.Tasks;
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
                Logger.LogMessage($"Iniciando processo de login para: {email}");

                initiateSignIn(
                    email,
                    password,
                    () =>
                    {
                        Logger.LogMessage("Login realizado com sucesso");
                        onSuccess?.Invoke("Login successful");
                    },
                    (error) =>
                    {
                        Logger.LogMessage($"Erro na criação do login: {error}");
                        onError?.Invoke(error);
                    },
                    (error) =>
                    {
                        Logger.LogMessage($"Erro na atualização do login: {error}");
                        onError?.Invoke(error);
                    }
                );
            }
            catch (Exception ex)
            {
                Logger.LogMessage($"Exceção durante o login: {ex.Message}");
                onError?.Invoke(new Error(
                    ErrorType.UnknownError,
                    "500",
                    "UNEXPECTED_ERROR",
                    $"Ocorreu um erro inesperado: {ex.Message}"
                ));
            }

        }
    }
}
