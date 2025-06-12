using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RabbitWings.Core
{
    [Serializable]
    public class MissionAndNetData<T, W, C> where T : AbstractByMissionAndNetData<W, C>, new() where W : IEquatable<W>, new() where C : ICollection
    {
        public bool Merge(MissionAndNetData<T, W, C> other)
        {
            bool changed = false;
            foreach (string mcs in other.Data.Keys)
            {
                T mc = other.Data[mcs];
                T myc = GetMissionData(mc.MissionName, mc.NetworkID);
                foreach (W m in mc.LoopThrough())
                {
                    if (myc.Contains(m))
                    {
                        changed |= myc.Merge(myc.Get(m), m);
                    }
                    else
                    {
                        myc.Add(m);
                        changed = true;
                    }
                }
            }
            return changed;
        }
        protected Dictionary<string, T> missionData;
        public Dictionary<string, T> Data
        {
            get
            {
                if (missionData == null)
                {
                    missionData = new Dictionary<string, T>();
                }
                return missionData;
            }
        }
        public bool IsDirty
        {
            get
            {
                foreach(T d in Data.Values)
                {
                    if (d.IsDirty) return true;
                }
                return false;
            }
            set
            {
                foreach (T d in Data.Values)
                {
                    if (d.IsDirty != value) d.IsDirty = value;
                }
            }
        }
        public void Add(W dataBit)
        {
            T myT = GetMissionData();
            if (myT == null) { throw new NullReferenceException("No current Mission..."); }
            myT.Add(dataBit);
        }
        public W Get(W itm)
        {
            T myT = GetMissionData();
            if (myT == null) { throw new NullReferenceException("No current Mission..."); }
            return myT.Get(itm);
        }

        public T GetMissionData()
        {
            if (GlobalSettings.Instance.MissionAndNetProvider == null || string.IsNullOrEmpty(GlobalSettings.Instance.MissionAndNetProvider.MissionName)) { return null; }
            string mission = GlobalSettings.Instance.MissionAndNetProvider.MissionName;
            string netId = GlobalSettings.Instance.MissionAndNetProvider.NetName;
            return GetMissionData(mission, netId);
        }

        public T GetMissionData(string mission, string netId)
        {
            string mnid = $"{mission}-{netId}";
            if (Data.ContainsKey(mnid))
            {
                return Data[mnid];
            }
            T mcd = new()
            {
                MissionName = mission,
                NetworkID = netId
            };
            Data.Add(mnid, mcd);
            return mcd;
        }
    }
}
