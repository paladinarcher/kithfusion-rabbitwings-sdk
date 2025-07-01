using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace RabbitWings.Core
{
    public abstract class RestJsonCacheGeneric<T> : RestJsonGeneric<IdObjectReference<T>, IdObjectReference<T>> where T : class
    {
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
            GetCache(key, (IdObjectReference<T> r) => { callback.Invoke(r.myObject); }, onError);
        }

        public void SetCache(string key, T value, Action<T> onComplete, Action<Error> onError)
        {
            IdObjectReference<T> m = new()
            {
                ID = key,
                myObject = value
            };
            SetCache(m, onComplete, onError);
        }

        public void SetCache(IdObjectReference<T> myObject, Action<T> onCompleted, Action<Error> onError)
        {
            SetCache(myObject, (IdObjectReference<T> r) => { onCompleted.Invoke(r.myObject); }, onError);
        }
    }
}
