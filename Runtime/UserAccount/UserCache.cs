using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RabbitWings.Core;

namespace RabbitWings.UserAccount
{
    public class UserCache : RestJsonCacheGeneric<User>
    {
        public float timeBetweenCaching = 120f;
        protected Coroutine caching;
        protected bool refreshing = false;

        public override string GetIDPrefix
        {
            get
            {
                Type t = typeof(User);
                return t.Name;
            }
        }
        public void CacheCurrentUser()
        {
            if (User.Current == null) { return; }
            CacheCurrentUser((User u) => {
                Debug.Log($"User cached: {u}");
            }, (Error e) => {
                Debug.LogError($"Error caching user: {e}");
            });
        }
        public void CacheCurrentUser(Action<User> onComplete, Action<Error> onError)
        {
            if (User.Current == null)
            {
                onError?.Invoke(new Error(ErrorType.UnknownError, "404", "404", "Current User doesn't exist yet."));
                return;
            }
            SetCache(GetID(User.Current), User.Current, (User usr) => {
                onComplete?.Invoke(User.Current);
            }, onError);
        }

        protected override string GetDefaultPath()
        {
            return "user";
        }

        protected string GetID(User current)
        {
            return $"{GetIDPrefix}-{current.email}";
        }
    }
}
