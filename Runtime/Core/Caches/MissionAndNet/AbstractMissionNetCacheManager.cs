using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RabbitWings.Core
{
    public abstract class AbstractMissionNetCacheManager<V, T, W, C> : MonoBehaviour where V : MissionAndNetData<T, W, C> where T : AbstractByMissionAndNetData<W, C>, new() where W : IEquatable<W>, new() where C : ICollection
    {
        public static AbstractMissionNetCacheManager<V, T, W, C> Instance
        {
            get; protected set;
        }

        public float pollingRateSeconds = 0.5f;
        public UnityEvent onChanged;

        protected Coroutine watcher;
        public abstract V Data { get; set; }

        protected virtual void Awake()
        {
            Instance = this;
        }

        protected virtual void Start()
        {
            watcher = StartCoroutine(WatchData());
        }

        protected IEnumerator WatchData()
        {
            while (true)
            {
                yield return new WaitForSeconds(pollingRateSeconds);
                if (Data != null && Data.IsDirty)
                {
                    onChanged?.Invoke();
                }
            }
        }

        public abstract void Add(W bit);

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

    }
}
