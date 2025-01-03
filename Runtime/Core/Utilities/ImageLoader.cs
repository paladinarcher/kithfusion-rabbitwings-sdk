using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

namespace RabbitWings.Core.Utilities
{
    /// <summary>
    /// Manages asynchronous loading and caching of sprites from URLs
    /// </summary>
    public class ImageLoader : MonoBehaviour
    {
        private static ImageLoader _instance;
        private static readonly Dictionary<string, Sprite> SpriteCache = new Dictionary<string, Sprite>();
        private static readonly Dictionary<string, List<Action<Sprite>>> PendingCallbacks = new Dictionary<string, List<Action<Sprite>>>();

        public static ImageLoader Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("[ImageLoader]");
                    _instance = go.AddComponent<ImageLoader>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Loads a sprite from a URL with caching support
        /// </summary>
        /// <param name="url">The URL of the image to load</param>
        /// <param name="callback">Callback to receive the loaded sprite</param>
        /// <exception cref="ArgumentNullException">Thrown when URL is null or empty</exception>
        public void LoadSprite(string url, Action<Sprite> callback)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException(nameof(url));

            // Return cached sprite if available
            if (SpriteCache.TryGetValue(url, out Sprite cachedSprite) && cachedSprite != null)
            {
                callback?.Invoke(cachedSprite);
                return;
            }

            // Add callback to pending list
            if (!PendingCallbacks.ContainsKey(url))
            {
                PendingCallbacks[url] = new List<Action<Sprite>>();
                StartCoroutine(LoadSpriteCoroutine(url));
            }

            if (callback != null)
            {
                PendingCallbacks[url].Add(callback);
            }
        }

        private IEnumerator LoadSpriteCoroutine(string url)
        {
            using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                    Sprite sprite = Sprite.Create(
                        texture,
                        new Rect(0, 0, texture.width, texture.height),
                        new Vector2(0.5f, 0.5f),
                        100f,
                        0,
                        SpriteMeshType.FullRect);

                    // Cache the sprite
                    SpriteCache[url] = sprite;

                    // Notify all pending callbacks
                    if (PendingCallbacks.TryGetValue(url, out var callbacks))
                    {
                        foreach (var callback in callbacks)
                        {
                            callback?.Invoke(sprite);
                        }
                        PendingCallbacks.Remove(url);
                    }
                }
                else
                {
                    Debug.LogError($"Failed to load image from {url}: {request.error}");
                    // Remove failed request from pending callbacks
                    PendingCallbacks.Remove(url);
                }
            }
        }

        /// <summary>
        /// Clears the sprite cache
        /// </summary>
        public void ClearCache()
        {
            SpriteCache.Clear();
        }

        /// <summary>
        /// Removes a specific sprite from the cache
        /// </summary>
        /// <param name="url">The URL of the sprite to remove</param>
        public void RemoveFromCache(string url)
        {
            if (SpriteCache.ContainsKey(url))
            {
                SpriteCache.Remove(url);
            }
        }

        private void OnDestroy()
        {
            ClearCache();
            PendingCallbacks.Clear();
        }
    }
}