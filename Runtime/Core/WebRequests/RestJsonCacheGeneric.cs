using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace RabbitWings.Core
{
    public abstract class RestJsonCacheGeneric<T> : MonoBehaviour where T : class
    {
        public string prodUrl;
        public string devUrl;
        public string prodApiKey;
        public string devApiKey;

        protected string apiKey;

        protected string url;

        protected Coroutine runner;

        protected abstract string GetDefaultPath();

        protected virtual void Awake()
        {

        }
        protected virtual void Start()
        {
            if (GlobalSettings.Instance.IsProd)
            {
                url = string.IsNullOrEmpty(prodUrl) ? GlobalSettings.Instance.DefaultProdUrl : prodUrl;
                apiKey = string.IsNullOrEmpty(prodApiKey) ? GlobalSettings.Instance.DefaultProdApiKey : prodApiKey;
            }
            else
            {
                url = string.IsNullOrEmpty(devUrl) ? GlobalSettings.Instance.DefaultDevUrl : devUrl;
                apiKey = string.IsNullOrEmpty(devApiKey) ? GlobalSettings.Instance.DefaultDevApiKey : devApiKey;
            }
        }

        public virtual string GetIDPrefix
        {
            get
            {
                return ObjectType.Name;
            }
        }

        public Type ObjectType
        {
            get { return typeof(T); }
        }

        public void GetCache(string key, Action<T> callback, Action<Error> onError)
        {
            string getUrl = new UrlBuilder($"{url}/{GetDefaultPath()}/")
                .AddId(key)
                .AddType(ObjectType.Name)
                .Build();

            List<WebRequestHeader> headers = new List<WebRequestHeader>()
            {
                WebRequestHeader.AuthXApi(apiKey),
                WebRequestHeader.JsonContentTypeHeader()
            };

            WebRequestHelper.Instance.GetRequest(
                SdkType.Login,
                getUrl,
                headers,
                (IdObjectReference<T> r) => { callback.Invoke(r.myObject); },
                onError,
                ErrorGroup.CommonErrors);
        }

        public void SetCache(string key, T value, Action<T> onComplete, Action<Error> onError)
        {
            IdObjectReference<T> m = new IdObjectReference<T>();
            m.id = key;
            m.myObject = value;
            m.type = value.GetType().Name;
            SetCache(m, onComplete, onError);
        }

        public void SetCache(IdObjectReference<T> myObject, Action<T> onCompleted, Action<Error> onError)
        {
            string postUrl = new UrlBuilder($"{url}/{urlPath}/")
                .Build();

            List<WebRequestHeader> headers = new List<WebRequestHeader>()
            {
                WebRequestHeader.AuthXApi(apiKey),
                WebRequestHeader.JsonContentTypeHeader()
            };

            WebRequestHelper.Instance.PostRequest(
                SdkType.Login,
                postUrl,
                myObject,
                headers,
                (IdObjectReference<T> r) => { onCompleted.Invoke(r.myObject); },
                onError,
                ErrorGroup.CommonErrors);
        }
    }
}
