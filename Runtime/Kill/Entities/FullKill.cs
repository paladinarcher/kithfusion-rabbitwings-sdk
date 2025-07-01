using Newtonsoft.Json;
using RabbitWings.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RabbitWings.Kill
{
    public class FullKill : Kill
    {
        public override string Type
        {
            get
            {
                return typeof(Kill).Name;
            }
        }
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("first_kill_seconds_from_start")]
        public float FirstKillSecondsFromStart { get; set; }

        [JsonProperty("first_timestamp")]
        public string FirstTimestamp { get; set; }

        [JsonProperty("seconds_since_start")]
        public float SecondsSinceStart { get; set; }
    }
}