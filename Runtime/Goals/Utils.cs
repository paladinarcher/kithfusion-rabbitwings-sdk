using RabbitWings.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RabbitWings.Goals
{
    public static class Utils
    {
        private static string baseDirectory;

        public static string Url
        {
            get
            {
                return UtilsBase.Url;
            }
        }
        public static string ApiKey
        {
            get
            {
                return UtilsBase.ApiKey;
            }
        }
        public static string BaseDirectory
        {
            get
            {
                if (baseDirectory == null)
                {
                    baseDirectory = "user/goals";
                }
                return baseDirectory;
            }
        }
        public static void GetState(Action<GoalStateSummary> onComplete, Action<Error> onError)
        {
            GetState(null, onComplete, onError);
        }
        public static void GetState(int? id, Action<GoalStateSummary> onComplete, Action<Error> onError)
        {
            string getUrl = new UrlBuilder($"{Url}/{BaseDirectory}")
                .AddId(id.ToString())
                .AddType(nameof(GoalStateSummary))
                .Build();

            List<WebRequestHeader> headers = new List<WebRequestHeader>()
            {
                WebRequestHeader.AuthXApi(ApiKey),
                WebRequestHeader.JsonContentTypeHeader(),
                WebRequestHeader.CurrentUser()
            };

            WebRequestHelper.Instance.GetRequest(
                SdkType.Login,
                getUrl,
                headers,
                (GoalStateSummary r) => { onComplete?.Invoke(r); },
                onError,
                ErrorGroup.CommonErrors);
        }
        public static void SetState(GoalIndexCountStatus gc, Action onComplete, Action<Error> onError)
        {
            GoalStateSummary gs = new(1);
            gs.Add(gc);
            SetState(gs, onComplete, onError);
        }

        public static void SetState(GoalStateSummary gs, Action onComplete, Action<Error> onError)
        {

            string postUrl = new UrlBuilder($"{Url}/{BaseDirectory}")
                .Build();

            List<WebRequestHeader> headers = new List<WebRequestHeader>()
            {
                WebRequestHeader.AuthXApi(ApiKey),
                WebRequestHeader.JsonContentTypeHeader(),
                WebRequestHeader.CurrentUser()
            };


            WebRequestHelper.Instance.PostRequest(
                SdkType.Login,
                postUrl,
                gs,
                headers,
                () => { onComplete?.Invoke(); },
                onError,
                ErrorGroup.CommonErrors);
        }
    }
}
