using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RabbitWings.Core;

namespace RabbitWings.UserAccount
{
    public class LoginCache : BaseUserCache
    {
        public User CreateUser(string password, string email, string username)
        {
            User user = new User();
            user.email = email;
            user.username = username;
            user.password = password;
            return user;
        }
        public void LoginUser(string password, string email, string username, Action<User> onComplete, Action<Error> onError)
        {
            User user = CreateUser(password, email, username);
            LoginUser(user, onComplete, onError);
        }
        public void LoginUser(User user, Action<User> onComplete, Action<Error> onError)
        {
            SetCache(GetID(user), user, (User usr) => {
                User.Current = usr;
                onComplete?.Invoke(user);
            }, onError);
        }

        protected override string GetDefaultPath()
        {
            return "login";
        }
    }
}
