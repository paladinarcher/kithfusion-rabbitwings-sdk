using Newtonsoft.Json;
using RabbitWings.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RabbitWings.Kill
{
    public class KillPoints : IdObjectReference<Dictionary<string, KillPoint>>
    {
        public const string MAIN_ID = "KillGUIDToPoints";
        public override string ID
        {
            get
            {
                return MAIN_ID;
            }
            set
            {
                // no setting this
            }
        }
        public override string Type { get { return "KillPoints"; } }

    }

    public class KillPoint
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("points")]
        public int Points { get; set; }
    }
}