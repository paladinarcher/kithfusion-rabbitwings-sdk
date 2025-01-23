using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RabbitWings.Core;

namespace RabbitWings.UserAccount
{
    public abstract class BaseUserCache : RestJsonCacheGeneric<User>
    {
        public override string GetIDPrefix
        {
            get
            {
                Type t = typeof(User);
                return t.Name;
            }
        }

        protected override string GetDefaultPath()
        {
            return "editUser";
        }

        protected string GetID(User current)
        {
            return $"{GetIDPrefix}-{current.email}";
        }
    }
}
