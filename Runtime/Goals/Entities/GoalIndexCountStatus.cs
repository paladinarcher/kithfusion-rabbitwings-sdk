using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RabbitWings.Goals
{
    [Serializable]
    public class GoalIndexCountStatus : IEquatable<GoalIndexCountStatus>
    {
        public int GoalIndex;
        public int Count;
        public GoalState state;

        public bool Equals(GoalIndexCountStatus other)
        {
            return other.GoalIndex == GoalIndex;
        }
    }
}
