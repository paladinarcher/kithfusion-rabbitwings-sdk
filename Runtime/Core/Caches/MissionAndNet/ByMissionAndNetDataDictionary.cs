using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RabbitWings.Core
{
    public abstract class ByMissionAndNetDataDictionary<T> : AbstractByMissionAndNetData<T, Dictionary<string, T>> where T : IStringIndex<T>
    {
        private Dictionary<string, T> items;
        protected int initialSize = 0;

        public override Dictionary<string, T> Data
        {
            get
            {
                items ??= new Dictionary<string, T>(initialSize);
                return items;
            }
        }
        public override int Count => Data.Values.Count;

        public override IEnumerable<T> LoopThrough()
        {
            return Data.Values;
        }

        public override bool Contains(T item)
        {
            return Data.ContainsKey(item.UID);
        }

        public override void Add(T item)
        {
            bool changed = false;
            if (Data.ContainsKey(item.UID))
            {
                changed = Merge(Data[item.UID], item);
            } else
            {
                Data.Add(item.UID, item);
                changed = true;
            }
            if (changed) { IsDirty = true; }
        }

        public override T Get(T item)
        {
            if (!Data.ContainsKey(item.UID))
            {
                return item;
            }
            return Data[item.UID];
        }
    }
}
