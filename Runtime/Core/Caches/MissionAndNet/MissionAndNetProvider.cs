using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RabbitWings.Core
{
    public abstract class MissionAndNetProvider : MonoBehaviour
    {
        public abstract string MissionName { get; }
        public abstract string NetName { get; }
    }
}
