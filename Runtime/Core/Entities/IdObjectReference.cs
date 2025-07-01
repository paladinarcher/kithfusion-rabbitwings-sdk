using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RabbitWings.Core
{
    public class IdObjectReference<Y> : IDType where Y : class
    {
        public Y myObject;
        public override string Type
        {
            get
            {
                return typeof(Y).Name;
            }
        }
    }
}
