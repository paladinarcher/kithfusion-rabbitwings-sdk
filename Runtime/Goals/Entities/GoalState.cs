using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RabbitWings.Goals
{
    [Serializable]
    public enum GoalState
    {
        Empty,
        Cancelled,
        Initialized,
        Starting,
        IntentionalPauseStarting,
        PausedStarting,
        WaitingToStart,
        Started,
        Completing,
        IntentionalPauseCompleting,
        PausedCompleting,
        WaitingToEnd,
        Completed
    }
}