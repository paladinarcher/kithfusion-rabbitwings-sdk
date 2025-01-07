using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace RabbitWings.Core
{
    public partial class WebRequestHelper
    {
        public void AudioRequest(string url, Action<AudioClip> onComplete, Action<Error> onError)
        {
            StartCoroutine(PerformAudio(url, onComplete, onError));
        }

        private IEnumerator PerformAudio(string url, Action<AudioClip> onComplete, Action<Error> onError)
        {
            AudioType at = AudioType.WAV;
            if (url.Contains(".mp3")) { at = AudioType.MPEG; }
            var webRequest = UnityWebRequestMultimedia.GetAudioClip(url, at);
            yield return StartCoroutine(PerformWebRequest<AudioClip>(webRequest, onComplete, onError, ErrorGroup.CommonErrors));
        }
    }
}