using RabbitWings.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RabbitWings.Core
{
    public class GlobalSettings : MonoBehaviour
    {
        private static GlobalSettings _instance;

        public static GlobalSettings Instance
        {
            get
            {
                if (_instance == null)
                    _instance = WebRequestHelper.Instance.gameObject.AddComponent<GlobalSettings>();

                return _instance;
            }
        }

        public bool IsProd
        {
            get; set;
        } = false;
        public string DefaultProdUrl { get; set; } = "https://nr6y7hmhaj.execute-api.us-east-2.amazonaws.com/prod/rabbit-wings";
        public string DefaultDevUrl { get; set; } = "https://vrosr3etxd.execute-api.us-east-2.amazonaws.com/dev/rabbit-wings";
        public string DefaultProdApiKey { get; set; } = "MakeThesesRabbitsFly";
        public string DefaultDevApiKey { get; set; } = "MakeThesesRabbitsFly";
    }
}