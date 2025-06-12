using RabbitWings.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RabbitWings.Goals
{
    [Serializable]
    public class GoalStateSummary : ByMissionAndNetDataDictionary<GoalIndexCountStatus>
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
            this.initialSize = initialCount;
        }
        public event Action<Dictionary<string, GoalIndexCountStatus>> OnStatesUpdated;
        public void SetState(GoalIndexCountStatus[] states)
        {
            foreach(GoalIndexCountStatus i in states)
            {
                Add(i);
            }
        }
        public void SetState(Dictionary<string, GoalIndexCountStatus> states)
        {
            foreach(GoalIndexCountStatus i in states.Values)
            {
                Add(i);
            }
        }
        public void SetState(List<GoalIndexCountStatus> states)
        {
            foreach (GoalIndexCountStatus i in states)
            {
                Add(i);
            }
        }

        public bool Equals(GoalStateSummary other)
        {
            return true;
        }

        public Dictionary<string, GoalIndexCountStatus> UpdateFrom(GoalStateSummary gs)
        {
            Dictionary<string, GoalIndexCountStatus> diff = new Dictionary<string, GoalIndexCountStatus>();
            foreach(string i in gs.Data.Keys)
            {
                if (!Data.ContainsKey(i))
                {
                    diff.Add(i, gs.Data[i]);
                    Add(gs.Data[i]);
                    continue;
                }
                bool changed = Merge(Data[i], gs.Data[i]);
                if (changed) {
                    diff.Add(i, gs.Data[i]);
                }
            }

            return diff;
        }

        public override bool Merge(GoalIndexCountStatus existingItem, GoalIndexCountStatus newItem)
        {
            bool changed = false;
            if (existingItem.Count != newItem.Count)
            {
                existingItem.Count = newItem.Count;
                changed = true;
            }
            if (existingItem.state != newItem.state)
            {
                existingItem.state = newItem.state;
                changed = true;
            }
            return changed;
        }
    }
}