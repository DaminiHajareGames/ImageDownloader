using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace CommonOfDamini.Downloader
{
    public class ImageDownloader : MonoBehaviour
    {
        public static ImageDownloader GetInstance()
        {
            GameObject imgDownloader = new GameObject("ImageDownloader");
            return imgDownloader.AddComponent<ImageDownloader>();
        }

        public static ImageDownloader GetInstance(string indentifier)
        {
            GameObject imgDownloader = new GameObject(indentifier);
            return imgDownloader.AddComponent<ImageDownloader>();
        }

        public void LoadImageFromURL(string url, Action<bool, Sprite> callback)
        {
            StartCoroutine(DownloadImage(url, callback));
        }

        IEnumerator DownloadImage(string url, Action<bool, Sprite> callback)
        {
            Debug.Log($"DownloadImage : {url}");

            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
                callback?.Invoke(false, null);
            }
            else
            {

                try
                {
                    var texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one / 2);
                    callback?.Invoke(true, sprite);
                }
                catch (Exception)
                {
                    callback?.Invoke(false, null);
                }

            }
            Destroy(this.gameObject);
        }

    }
}