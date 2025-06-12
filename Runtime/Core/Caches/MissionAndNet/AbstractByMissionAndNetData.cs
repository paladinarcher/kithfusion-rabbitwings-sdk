using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace RabbitWings.Core
{
    public abstract class AbstractByMissionAndNetData<T, W> : IEquatable<ByMissionAndNetData<T>> where T : IEquatable<T> where W : ICollection
    {
        public string MissionName;
        public string NetworkID;

        public abstract W Data { get; }

        [JsonIgnore] public virtual bool IsDirty { get; set; }

        [JsonIgnore] public abstract int Count { get; }

        public abstract IEnumerable<T> LoopThrough();

        public abstract bool Contains(T item);

        public abstract void Add(T item);

        public abstract T Get(T item);

        public abstract bool Merge(T existingItem, T newItem);

        public bool Equals(ByMissionAndNetData<T> other)
        {
            return MissionName == other.MissionName && NetworkID == other.NetworkID;
        }

        [OnSerialized]
        internal void OnSerialized(StreamingContext context)
        {
            IsDirty = false;
        }
    }
}