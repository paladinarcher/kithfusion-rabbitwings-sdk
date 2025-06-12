using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RabbitWings.Core {
    public interface IStringIndex<T> : IEquatable<T> 
    {
        public string UID { get; }
    }

}
