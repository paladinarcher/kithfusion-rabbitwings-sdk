using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RabbitWings.Core
{
    public abstract class IDType : IEquatable<IDType>
    {
        [JsonProperty("id")]
        public virtual string ID { get; set; }
        [JsonProperty("type")]
        public virtual string Type
        {
            get
            {
                return GetType().Name;
            }
        }
        public bool Equals(IDType other)
        {
            return other != null && ID.Equals(other.ID) && Type.Equals(other.Type);
        }
    }
}