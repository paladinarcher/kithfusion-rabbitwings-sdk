using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RabbitWings.Core;

namespace RabbitWings.UserAccount
{
    public class RegisterationCache : BaseUserCache
    {
        public User CreateUser(string firstname, string lastname, string password, string email, string username)
        {
            User user = new User();
            user.first_name = firstname;
            user.last_name = lastname;
            user.email = email;
            user.username = username;
            user.password = password;
            user.nickname = username;
            return user;
        }
        public void RegisterUser(string firstname, string lastname, string password, string email, string username, Action<User> onComplete, Action<Error> onError)
        {
            User user = CreateUser(firstname, lastname, password, email, username);
            RegisterUser(user, onComplete, onError);
        }
        public void RegisterUser(User user, Action<User> onComplete, Action<Error> onError)
        {
            SetCache(GetID(user), user, (User usr) => {
                User.Current = usr;
                onComplete?.Invoke(usr);
            }, onError);
        }

        protected override string GetDefaultPath()
        {
            return base.GetDefaultPath()+"/register";
        }
    }
}
