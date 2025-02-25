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
                states = new Dictionary<string, GoalIndexCountStatus>(initialCount);
            } else
            {
                states = new Dictionary<string, GoalIndexCountStatus>();
            }
        }

        public Dictionary<string, GoalIndexCountStatus> states;
        public void SetState(GoalIndexCountStatus[] states)
        {
            this.states = new Dictionary<string, GoalIndexCountStatus>(states.Length);
            foreach(GoalIndexCountStatus i in states)
            {
                this.states.Add(i.GoalIndex.ToString(), i);
            }
        }
        public void SetState(Dictionary<string, GoalIndexCountStatus> states)
        {
            this.states = states;
        }
        public void SetState(List<GoalIndexCountStatus> states)
        {
            this.states = new Dictionary<string, GoalIndexCountStatus>(states.Count);
            foreach (GoalIndexCountStatus i in states)
            {
                this.states.Add(i.GoalIndex.ToString(), i);
            }
        }

        public bool Equals(GoalStateSummary other)
        {
            return true;
        }
    }
}