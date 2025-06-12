using RabbitWings.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RabbitWings.Goals
{
    public class GoalDataManager : AbstractMissionNetCacheManager<GoalData, GoalStateSummary, GoalIndexCountStatus, Dictionary<string, GoalIndexCountStatus>> 
    {
        protected GoalData _data;
        public override GoalData Data
        {
            get
            {
                _data ??= new GoalData();
                return _data;
            }
            set
            {
                if (_data == null)
                {
                    _data = value;
                } else
                {
                    _data.Merge(value);
                }
            }
        }

        public override void Add(GoalIndexCountStatus bit)
        {
            Data.Add(bit);
        }

        public void SetState(GoalIndexCountStatus status)
        {
            Add(status);
        }
    }
}