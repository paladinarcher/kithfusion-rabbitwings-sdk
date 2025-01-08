using System;

namespace RabbitWings.Core
{
    public static class Settings
    {
        private static string _baseUrl;

        public static string BaseUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_baseUrl))
                    throw new InvalidOperationException("Base URL has not been set");
                return _baseUrl;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException(nameof(value), "Base URL cannot be null or empty");
                _baseUrl = value.TrimEnd('/');
            }
        }

        static Settings()
        {
            BaseUrl = "https://kithfusioncache.azurewebsites.net";
        }
    }
}