using RabbitWings.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RabbitWings.Kill
{
    public class KillPointCache : RestJsonGeneric<KillPoints, KillPoints>
    {
        public static KillPoints Items { get; set; }
        public static event Action OnLoad;

        public int GetPointsByKillGuid(string killGuid)
        {
            if(Items == null) { throw new KeyNotFoundException($"Kill Point Cache not loaded yet."); }
            foreach (string guid in Items.myObject.Keys)
            {
                if (guid.Equals(killGuid))
                {
                    return Items.myObject[guid].Points;
                }
            }
            throw new ArgumentException($"Kill Point Cache has no GUID {killGuid}");
        }

        public void SetCache(Action<KillPoints> onComplete, Action<Error> onError)
        {
            if (Items == null)
            {
                onError.Invoke(new Error(ErrorType.InvalidData, "", "", "No Kill Points available to cache"));
                return;
            }

            SetCache(Items, onComplete, onError);
        }

        public void GetCache(Action<KillPoints> onComplete, Action<Error> onError)
        {
            GetCache(KillPoints.MAIN_ID, (KillPoints i) => {
                Items = i;
                OnLoad?.Invoke(); onComplete?.Invoke(i);
            }, onError);
        }
        protected override string GetDefaultPath()
        {
            return "cache";
        }
    }
}
