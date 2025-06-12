using Newtonsoft.Json;
using RabbitWings.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RabbitWings.Goals
{
    [Serializable]
    public class GoalIndexCountStatus : IStringIndex<GoalIndexCountStatus>, IEquatable<GoalIndexCountStatus>
    {
        public int GoalIndex;
        public int Count;
        public GoalState state;

        [JsonIgnore]
        public string UID
        {
            get
            {
                return GoalIndex.ToString();
            }
        }

        public bool Equals(GoalIndexCountStatus other)
        {
            return other.GoalIndex == GoalIndex;
        }
    }
}
