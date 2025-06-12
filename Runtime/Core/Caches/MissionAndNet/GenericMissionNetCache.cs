using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RabbitWings.Core
{
    public abstract class GenericMissionNetCache<C, M, T, W, TC> : GenericCache<C> where C : MissionAndNetData<T, W, TC> where T : AbstractByMissionAndNetData<W, TC>, new() where W : IEquatable<W>, new() where M : AbstractMissionNetCacheManager<C, T, W, TC> where TC : ICollection
    {
        public M manager;
        public bool pullInitialOnLogin = true;
        public UnityEvent onSuccessCache;
        public UnityErrorEvent onFailureCache;

        private bool initialized = false;
        protected override void Awake()
        {
            base.Awake();
            if (manager == null)
            {
                manager = GetComponent<M>();
            }
        }
        protected override void Start()
        {
            base.Start();
            User.OnCurrentUserLoggedIn += (User u) => {
                Initialize();
                if (pullInitialOnLogin)
                {
                    GetCache(GetID(GetIDPrefix, User.Current), (C c) =>
                    {
                        Debug.Log($"{GetType()}.Initialize <GetCache.onSuccess> {JsonConvert.SerializeObject(c)}");
                        manager.Data = c;
                        manager.Data.IsDirty = false;
                    }, (Error e) =>
                    {
                        Debug.LogError($"Error getting {typeof(C)}: {e}");
                    });
                }
            };
        }
        public void Initialize()
        {
            if (manager != null)
            {
                if (!initialized)
                {
                    manager.onChanged.AddListener(() =>
                    {
                        CacheCurrent(manager.Data, (C c) =>
                        {
                            Debug.Log($"{typeof(C)} cached: {c}");
                        }, (Error e) =>
                        {
                            Debug.LogError($"Error caching {typeof(C)}: {e}");
                        });
                    });
                }
                initialized = true;
            }
        }
        public virtual void CacheCurrent(C c, Action<C> onComplete, Action<Error> onError)
        {
            Debug.Log($"{GetType()}: caching {c}");
            SetCache(GetID(GetIDPrefix, User.Current), c, (C usr) => {
                onComplete?.Invoke(usr);
                onSuccessCache?.Invoke();
            }, (Error e) => {
                onError.Invoke(e);
                onFailureCache.Invoke(e);
            });
        }

        protected virtual string GetID(string prefix, User current)
        {
            return $"{prefix}-{current.email.ToLower()}";
        }
    }
}
