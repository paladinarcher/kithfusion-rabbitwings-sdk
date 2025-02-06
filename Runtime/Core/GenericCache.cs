using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RabbitWings.Core
{
    public abstract class GenericCache<T> : RestJsonCacheGeneric<T> where T : class
    {
        protected override string GetDefaultPath()
        {
            return "cache";
        }
    }
}
