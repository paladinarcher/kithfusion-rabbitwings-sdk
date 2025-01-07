using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace RabbitWings.Core
{
    public static class AudioLoader
    {
        private static readonly Dictionary<string, AudioClip> Clips = new Dictionary<string, AudioClip>();

        public static void LoadAudio(string url, Action<AudioClip> callback)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException(nameof(url));

            if (Clips.ContainsKey(url) && Clips[url] != null)
            {
                callback?.Invoke(Clips[url]);
                return;
            }

            if (!Clips.ContainsKey(url))
            {
                Clips[url] = null;

                WebRequestHelper.Instance.AudioRequest(
                    url,
                    clip => Clips[url] = clip,
                    error => XDebug.LogError(error.errorMessage));
            }

            CoroutinesExecutor.Run(WaitClip(url, callback));
        }

        private static IEnumerator WaitClip(string url, Action<AudioClip> callback)
        {
            yield return new WaitUntil(() => Clips[url] != null);
            callback?.Invoke(Clips[url]);
        }
    }
}