using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace RabbitWings.Core
{

    [Serializable]
    public abstract class ByMissionAndNetData<T> : AbstractByMissionAndNetData<T, List<T>> where T : IEquatable<T>
    {
        private List<T> items;

        public override List<T> Data
        {
            get
            {
                if (items == null)
                {
                    items = new List<T>();
                }
                return items;
            }
        }

        public override IEnumerable<T> LoopThrough()
        {
            return Data;
        }


        public override int Count => ((ICollection<T>)Data).Count;

        public T this[int index] { get => Data[index];
            set { 
                Data[index] = value;
                IsDirty = true;
            } 
        }

        public override bool Contains(T item)
        {
            return Data.Contains(item);
        }

        public override void Add(T item)
        {
            bool changed;
            if (!Data.Contains(item)) { 
                Data.Add(item);
                changed = true;
            }
            else
            {
                changed = Merge(Get(item), item);
            }
            if (changed) { IsDirty = true; }
        }

        public override T Get(T item)
        {
            foreach (T lmcd in Data)
            {
                if (item.Equals(lmcd))
                {
                    return lmcd;
                }
            }
            return item;
        }

        [OnDeserializing]
        internal void OnDeserializing(StreamingContext context)
        {
            items = new List<T>();
        }
    }
}