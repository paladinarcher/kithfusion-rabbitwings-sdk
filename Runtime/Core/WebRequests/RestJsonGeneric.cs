using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace RabbitWings.Core
{
    public abstract class RestJsonGeneric<T, W> : MonoBehaviour where T : IDType where W : IDType
    {
        public string prodUrl;
        public string devUrl;
        public string prodApiKey;
        public string devApiKey;
        public bool Pulling { get; protected set; } = false;

        public string Url
        {
            get
            {
                if (GlobalSettings.Instance.IsProd)
                {
                    return string.IsNullOrEmpty(prodUrl) ? GlobalSettings.Instance.DefaultProdUrl : prodUrl;
                }
                else
                {
                    return string.IsNullOrEmpty(devUrl) ? GlobalSettings.Instance.DefaultDevUrl : devUrl;
                }
            }
        }

        public string ApiKey
        {
            get
            {
                if (GlobalSettings.Instance.IsProd)
                {
                    return string.IsNullOrEmpty(prodApiKey) ? GlobalSettings.Instance.DefaultProdApiKey : prodApiKey;
                }
                else
                {
                    return string.IsNullOrEmpty(devApiKey) ? GlobalSettings.Instance.DefaultDevApiKey : devApiKey;
                }
            }
        }

        protected Coroutine runner;

        protected abstract string GetDefaultPath();

        protected virtual string GetDefaultPathForListGet() {
            return GetDefaultPath();
        }

        protected virtual void Awake()
        {
        }
        protected virtual void Start()
        {
        }

        protected virtual UrlBuilder SetAdditionalParams(UrlBuilder urlBuilder)
        {
            return urlBuilder;
        }


        public virtual void GetListFromCache(Action<List<W>> callback, Action<Error> errorCallback)
        {
            GetListFromCache(null, callback, errorCallback);
        }

        public virtual void GetListFromCache(Dictionary<string, string> additionalParams, Action<List<W>> callback, Action<Error> errorCallback)
        {
            UrlBuilder ub = new UrlBuilder($"{Url}/{GetDefaultPathForListGet()}");
            if (additionalParams != null)
            {
                foreach (string key in additionalParams.Keys)
                {
                    ub.AddParam(key, additionalParams[key]);
                }
            }
            string getUrl = SetAdditionalParams(ub).Build();

            List<WebRequestHeader> headers = new List<WebRequestHeader>()
            {
                WebRequestHeader.AuthXApi(ApiKey),
                WebRequestHeader.JsonContentTypeHeader(),
                WebRequestHeader.CurrentUser()
            };

            Pulling = true;
            WebRequestHelper.Instance.GetRequest(
                SdkType.Login,
                getUrl,
                headers,
                (List<W> r) => { callback.Invoke(r); Pulling = false; },
                errorCallback,
                ErrorGroup.CommonErrors);
        }

        public virtual void GetCache(string key, Action<W> callback, Action<Error> onError)
        {
            string getUrl = new UrlBuilder($"{Url}/{GetDefaultPath()}")
                .AddId(key)
                .AddType(typeof(W).Name)
                .Build();

            List<WebRequestHeader> headers = new List<WebRequestHeader>()
            {
                WebRequestHeader.AuthXApi(ApiKey),
                WebRequestHeader.JsonContentTypeHeader(),
                WebRequestHeader.CurrentUser()
            };
            Pulling = true;
            WebRequestHelper.Instance.GetRequest(
                SdkType.Login,
                getUrl,
                headers,
                (W r) => { callback.Invoke(r); Pulling = false; },
                onError,
                ErrorGroup.CommonErrors);
        }

        public virtual void SetCache(T myObject, Action<W> onCompleted, Action<Error> onError)
        {
            string postUrl = new UrlBuilder($"{Url}/{GetDefaultPath()}")
                .Build();

            List<WebRequestHeader> headers = new List<WebRequestHeader>()
            {
                WebRequestHeader.AuthXApi(ApiKey),
                WebRequestHeader.JsonContentTypeHeader(),
                WebRequestHeader.CurrentUser()
            };

            WebRequestHelper.Instance.PostRequest(
                SdkType.Login,
                postUrl,
                myObject,
                headers,
                (W r) => { onCompleted.Invoke(r); },
                onError,
                ErrorGroup.CommonErrors);
        }
    }
}
