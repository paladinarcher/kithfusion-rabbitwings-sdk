using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RabbitWings.Core;

namespace RabbitWings.UserAccount
{
    public abstract class BaseUserCache : RestJsonCacheGeneric<User>
    {
        protected override string GetDefaultPath()
        {
            return "editUser";
        }

        public string GetID(User current)
        {
            return $"{GetIDPrefix}-{current.email.ToLower()}";
        }
    }
}
