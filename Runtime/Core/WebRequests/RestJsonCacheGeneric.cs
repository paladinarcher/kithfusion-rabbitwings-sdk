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

        protected virtual void Awake()
        {
        }
        protected virtual void Start()
        {
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
            string getUrl = new UrlBuilder($"{Url}/{GetDefaultPath()}")
                .AddId(key)
                .AddType(ObjectType.Name)
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
                (IdObjectReference<T> r) => { callback.Invoke(r.myObject); Pulling = false; },
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
                (IdObjectReference<T> r) => { onCompleted.Invoke(r.myObject); },
                onError,
                ErrorGroup.CommonErrors);
        }
    }
}
