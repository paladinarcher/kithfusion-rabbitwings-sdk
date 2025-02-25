using RabbitWings.Catalog;
using RabbitWings.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RabbitWings.Core
{
    public static class UtilsBase
    {
        private static string url;
        private static string apiKey;

        public static string Url
        {
            get
            {
                if (url == null)
                {
                    if (GlobalSettings.Instance.IsProd)
                    {
                        url = GlobalSettings.Instance.DefaultProdUrl;
                    }
                    else
                    {
                        url = GlobalSettings.Instance.DefaultDevUrl;
                    }
                }
                return url;
            }
        }
        public static string ApiKey
        {
            get
            {
                if (apiKey == null)
                {
                    if (GlobalSettings.Instance.IsProd)
                    {
                        apiKey = GlobalSettings.Instance.DefaultProdApiKey;
                    }
                    else
                    {
                        apiKey = GlobalSettings.Instance.DefaultDevApiKey;
                    }
                }
                return apiKey;
            }
        }
    }
}