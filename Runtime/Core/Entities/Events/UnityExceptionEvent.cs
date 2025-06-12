using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RabbitWings.Core
{
    [Serializable]
    public class UnityExceptionEvent : UnityEvent<Exception>
    {
    }
}
