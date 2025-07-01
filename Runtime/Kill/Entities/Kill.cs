using Newtonsoft.Json;
using RabbitWings.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RabbitWings.Kill
{
    [Serializable]
    public class Kill : IDType
    {
        [JsonProperty("id")]
        public override string ID
        {
            get
            {
                return $"{UserID}-{MissionName}-{NetworkID}-{KillGUID}";
            }
            set
            {
                //
            }
        }
        [JsonProperty("user_id")]
        public string UserID { get; set; }

        [JsonProperty("mission_name")]
        public string MissionName { get; set; }

        [JsonProperty("network_id")]
        public string NetworkID { get; set; }

        [JsonProperty("kill_guid")]
        public string KillGUID { get; set; }

        [JsonProperty("seconds_since_start")]
        public float SecondsFromStart { get; set; }
    }
}
