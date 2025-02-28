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
        public event Action<Dictionary<string, GoalIndexCountStatus>> OnStatesUpdated;
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

        public Dictionary<string, GoalIndexCountStatus> UpdateFrom(GoalStateSummary gs)
        {
            Dictionary<string, GoalIndexCountStatus> diff = new Dictionary<string, GoalIndexCountStatus>();
            foreach(string i in gs.states.Keys)
            {
                if (!states.ContainsKey(i))
                {
                    diff.Add(i, gs.states[i]);
                    states.Add(i, gs.states[i]);
                    continue;
                }
                if (states[i].Count != gs.states[i].Count || states[i].state != gs.states[i].state)
                {
                    states[i] = gs.states[i];
                    diff.Add(i, gs.states[i]);
                }
            }

            return diff;
        }
    }
}