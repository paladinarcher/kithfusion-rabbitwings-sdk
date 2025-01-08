using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
using RabbitWings.Core;

namespace RabbitWings.Account
{
    public static class LoginProto
    {

        public static void SignIn(string email, string password, Action onSuccess, Action<Error> onError, string redirectUri = null)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    onError?.Invoke(new Error(
                        "400",
                        "Parâmetros inválidos",
                        "BAD_REQUEST",
                        "Email ou senha não podem estar vazios"
                    ));
                    return;
                }

                XDebug.Log($"Initiating login process for user: {email}");

                var url = new UrlBuilder(Settings.BaseUrl)
                    .AddParam("path", "/api/loginHandler")
                    .Build();

                var signInRequest = new SignInRequest
                {
                    Email = email,
                    Password = password
                };

                XDebug.Log($"Preparing login request to URL: {url}");

                WebRequestHelper.Instance.PostRequest<SignInRequest, SignInRequest>(
                    SdkType.Login,
                    url,
                    signInRequest,
                    response =>
                    {
                        XDebug.Log($"Login successful");
                        onSuccess?.Invoke();
                    },
                    error =>
                    {
                        XDebug.LogError($"Login failed: {error}");
                        onError?.Invoke(error);
                    },
                    ErrorGroup.LoginErrors);
            }
            catch (Exception ex)
            {
                XDebug.LogError($"Unexpected error during login: {ex.Message}");
                onError?.Invoke(new Error(
                    "500",
                    "Erro do servidor",
                    "INTERNAL_SERVER_ERROR",
                    $"Ocorreu um erro: {ex.Message}"
                ));
            }
        }
       
        /// <summary>
        /// Adds a username, email address, and password, that can be used for authentication, to the current account.
        /// </summary>
        /// <remarks>[More about the use cases](https://developers.xsolla.com/sdk/unity/authentication/auth-via-device-id/).</remarks>
        /// <param name="username">Username.</param>
        /// <param name="password">User password.</param>
        /// <param name="email">User email.</param>
        /// <param name="onSuccess">Called after successful email and password linking.</param>
        /// <param name="onError">Called after the request resulted with an error.</param>
        /// <param name="redirectUri">URI to redirect the user to after account confirmation, successful authentication, two-factor authentication configuration, or password reset confirmation.
        ///     Must be identical to the OAuth 2.0 redirect URIs specified in Publisher Account.
        ///     Required if there are several URIs.</param>
        /// <param name="promoEmailAgreement">Whether the user gave consent to receive the newsletters.</param>
        public static void AddUsernameEmailAuthToAccount(
                string password,
                string email,
                Action onSuccess,
                Action onError,
                string redirectUri = null,
                int? promoEmailAgreement = null)
            {
                try
                {
                    var url = new UrlBuilder(Settings.BaseUrl)
                        .AddParam("path", "/api/loginHandler")
                        .Build();

                    var requestBody = new AddUsernameAndEmailRequest(password, email);

                    WebRequestHelper.Instance.PostRequest(
                        SdkType.Login,
                        url,
                        requestBody,
                        WebRequestHeader.AuthHeader(),
                        onSuccess,
                        error => TokenAutoRefresher.Check(
                            error,
                            onError,
                            () => AddUsernameEmailAuthToAccount(password, email, onSuccess, onError)
                        ),
                        ErrorGroup.RegistrationErrors
                    );
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Erro inesperado ao adicionar credenciais: {ex.Message}");
                    onError?.Invoke();
                }
            }


        public XsollaUser userObj = new XsollaUser();

        public void initiateSignIn(string email, string password, Action onSuccess, Action<Error> onSigninError)
        {
            userObj.email = email;
            if (!GameController.IsProdDB && string.IsNullOrEmpty(password)) { password = DEFAULT_DEBUG_PASSWORD; }
            userObj.password = password;
            GameController.AddLog("AuthManager", $"signing in {email}");
            Auth.SignIn(email, password, onSuccess: () => {
                //OnSignInSuccess();

                }, onError: (Error e) => {
                    OnError(e);
                    //onSigninError.Invoke(e);
                    relogin = false;
                });
            }, onError: (Error e) => {
                OnError(e);
                onSigninError.Invoke(e);
                relogin = false;
            });
        }

    public void initiateSignIn(string email, string password, Action onSuccess, Action onCreateError, Action onUpdateError)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            var error = new Error("400", "Parâmetros inválidos", "Email ou senha não podem estar vazios");
            onCreateError?.Invoke(error);
            return;
        }

        userObj.email = email;
        userObj.password = password;
        GameController.AddLog("AuthManager", $"signing in {userObj.email}");

        Auth.SignIn(() =>
        {
            Debug.Log($"Loggedin via Device");
            XsollaUserAccount.AddUsernameEmailAuthToAccount(userObj.password, userObj.email,
                (AddUsernameAndEmailResult added) =>
                {
                    Debug.Log($"Added {userObj.password}, {userObj.email} to account.");
                },
                (Error e) =>
                {
                    relogin = false;

                    // Mapeamento dos novos códigos de erro
                    switch (e.statusCode)
                    {
                        case "404":
                            Debug.LogError($"Usuário {userObj.email} não encontrado!");
                            break;
                        case "401":
                            Debug.LogError($"Credenciais inválidas para o usuário {userObj.email}!");
                            relogin = true;
                            break;
                        case "400":
                            Debug.LogError($"Parâmetros inválidos para o usuário {userObj.email}!");
                            break;
                        case "500":
                            Debug.LogError($"Erro do servidor ao processar a requisição para {userObj.email}!");
                            break;
                        default:
                            Debug.LogError($"Erro desconhecido: {e.statusCode}");
                            break;
                    }

                    XsollaAuth.Logout(() =>
                    {
                        Debug.Log($"Successfully backed out since we couldn't update the user with details given.");
                        if (relogin)
                        {
                            initiateSignIn(email, password, onSuccess, onUpdateError);
                        }
                    },
                    (Error er) =>
                    {
                        Debug.LogError($"Error logging out: {er}");
                    });

                    Debug.LogError(e);
                    OnError(e);
                    onUpdateError?.Invoke(e);
                });
        },
        (Error e) =>
        {
            Debug.LogError(e);
            OnError(e);
            onCreateError?.Invoke(e);
        });
    }

[Serializable]
        public class User
        {
            public string email;
            public string password;
            public string loginToken;
            public List<VirtualCurrencyBalance> vcurrencyBalances;
            public List<InventoryItem> inventoryItems;
            public GoalItemManager goalItems;
            public GoalManager.GoalStateSummary goalSummary;
            public string id;
            public string username;
            public string nickname;
            public string name;
            public string picture;
            public string birthday;
            public string first_name;
            public string last_name;
            public string gender;
            public string phone;
            public List<UserGroup> groups;
            public string registered;
            public string external_id;
            public string last_login;
            public UserBan ban;
            public string country;
            public string tag;
            [CanBeNull] public string connection_information;
            [CanBeNull] public bool? is_anonymous;
            [CanBeNull] public string phone_auth;
            /// <summary>
            /// User status. Can be 'online' or 'offline'.
            /// </summary>
            [CanBeNull] public string presence;
        }


    }



    