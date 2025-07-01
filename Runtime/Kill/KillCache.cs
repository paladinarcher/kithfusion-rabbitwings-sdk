using RabbitWings.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RabbitWings.Kill
{
    public class KillCache : RestJsonGeneric<Kill, FullKill>
    {
        public Kill GetKill(string KillGUID, float startTime)
        {
            Kill k = new()
            {
                KillGUID = KillGUID,
                MissionName = GlobalSettings.Instance.MissionAndNetProvider.MissionName,
                NetworkID = GlobalSettings.Instance.MissionAndNetProvider.NetName,
                SecondsFromStart = Time.time - startTime,
                UserID = User.Current.id
            };
            return k;
        }

        public void RegisterKill(string KillGUID, float startTime, Action<FullKill> callback, Action<Error> onError)
        {
            Kill k = GetKill(KillGUID, startTime);
            SetCache(k, callback, onError);
        }
        protected override string GetDefaultPath()
        {
            return "app/register-kill";
        }

        protected override string GetDefaultPathForListGet()
        {
            return "app/get-user-kills";
        }

        protected override UrlBuilder SetAdditionalParams(UrlBuilder urlBuilder)
        {
            return urlBuilder.AddMission(GlobalSettings.Instance.MissionAndNetProvider.MissionName).AddNetworkID(GlobalSettings.Instance.MissionAndNetProvider.NetName);
        }
    }
}
