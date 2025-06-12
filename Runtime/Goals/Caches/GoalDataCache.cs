using Newtonsoft.Json;
using RabbitWings.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RabbitWings.Goals
{
    public class GoalDataCache : GenericMissionNetCache<GoalData, GoalDataManager, GoalStateSummary, GoalIndexCountStatus, Dictionary<string, GoalIndexCountStatus>>
    {
    }


    //[Serializable]
    //public class GoalData : MissionAndNetData<MissionGoalData, GoalToState>
    //{
    //    public void Add(Goal g)
    //    {
    //        Add(g, () => { });
    //    }
    //    public void Add(Goal g, Action onChanged)
    //    {
    //        Debug.Log($"GoalData.Add {g.Index}({g.name}) => {g.State}");
    //        MissionGoalData m = GetMissionData();
    //        if (m != null)
    //        {
    //            GoalToState c = m.GetById(g.Index.ToString());
    //            c.state = g.State;
    //            onChanged?.Invoke();
    //        }
    //    }
    //}

    //[Serializable]
    //public class MissionGoalData : ByMissionAndNetData<GoalToState>
    //{
    //    public GoalToState GetById(string goalId)
    //    {
    //        foreach (GoalToState item in Data)
    //        {
    //            if (item.goalId == goalId) { return item; }
    //        }
    //        GoalToState c = new GoalToState();
    //        c.goalId = goalId;
    //        c.state = GoalState.Empty;
    //        Data.Add(c);
    //        return c;
    //    }

    //    public override bool Merge(GoalToState existingItem, GoalToState newItem)
    //    {
    //        if (existingItem.state != newItem.state)
    //        {
    //            existingItem.state = newItem.state;
    //            return true;
    //        }
    //        return false;
    //    }
    //}

    //[Serializable]
    //public class GoalToState : IEquatable<GoalToState>
    //{
    //    public string goalId;
    //    public GoalState state;

    //    public bool Equals(GoalToState other)
    //    {
    //        return goalId == other.goalId;
    //    }
    //}
}