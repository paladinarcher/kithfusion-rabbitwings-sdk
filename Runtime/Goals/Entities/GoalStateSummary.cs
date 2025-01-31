using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RabbitWings.Goals
{
    [Serializable]
    public class GoalStateSummary : IEquatable<GoalStateSummary>
    {
        public static GoalStateSummary Generate(GoalIndexCountStatus[] states)
        {
            GoalStateSummary s = new GoalStateSummary(0);
            s.SetState(states);
            return s;
        }
        public static GoalStateSummary Generate(List<GoalIndexCountStatus> states)
        {
            GoalStateSummary s = new GoalStateSummary(0);
            s.SetState(states);
            return s;
        }
        public GoalStateSummary() : this(1)
        {
        }
        public GoalStateSummary(int initialCount)
        {
            if (initialCount > 0)
            {
                states = new List<GoalIndexCountStatus>(initialCount);
            } else
            {
                states = new List<GoalIndexCountStatus>();
            }
        }

        public List<GoalIndexCountStatus> states;
        public void SetState(GoalIndexCountStatus[] states)
        {
            this.states = new List<GoalIndexCountStatus>(states);
        }
        public void SetState(List<GoalIndexCountStatus> states)
        {
            this.states = states;
        }

        public bool Equals(GoalStateSummary other)
        {
            return true;
        }
    }
}