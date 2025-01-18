using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace RabbitWings.Core
{
    public class RestJsonCacheGeneric<T> : MonoBehaviour where T : class
    {
        public static bool IS_PROD;
        public static string DEFAULT_PROD_URL;
        public static string DEFAULT_DEV_URL;
        public string prodUrl;
        public string devUrl;

        protected string url;

        protected Coroutine runner;

        protected virtual void Awake()
        {

        }
        protected virtual void Start()
        {
            if (IS_PROD)
            {
                url = string.IsNullOrEmpty(prodUrl) ? DEFAULT_PROD_URL : prodUrl;
            }
            else
            {
                url = string.IsNullOrEmpty(devUrl) ? DEFAULT_DEV_URL : devUrl;
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
            string getUrl = new UrlBuilder($"{url}/")
                .AddId(key)
                .AddType(ObjectType.Name)
                .Build();

            List<WebRequestHeader> headers = new List<WebRequestHeader>()
            {
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
            string postUrl = new UrlBuilder($"{url}/")
                .Build();

            List<WebRequestHeader> headers = new List<WebRequestHeader>()
            {
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
