using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RabbitWings.Core
{
    public class IdObjectReference<Y> : IEquatable<IdObjectReference<Y>> where T : class
    {
        public string id;
        public string type;
        public Y myObject;

        public bool Equals(IdObjectReference<Y> other)
        {
            return other != null && other.id == id && other.type == type;
        }
    }
}
