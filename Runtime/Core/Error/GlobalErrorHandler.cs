using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RabbitWings.Core
{
    public class GlobalErrorHandler : MonoBehaviour
    {
        public static GlobalErrorHandler Instance { get; protected set; }

        public UnityExceptionEvent OnException;

        private void Awake()
        {
            Instance = this;
        }

        public void Handle(Exception e)
        {
            OnException?.Invoke(e);
        }
    }
}
