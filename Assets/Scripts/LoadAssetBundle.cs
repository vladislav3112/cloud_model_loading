using System.Collections;
using UnityEngine.Networking;
using UnityEngine;
using System.IO;
public class LoadAssetBundle : MonoBehaviour
{
    private string bundleURL = "ftp://89.208.87.29/spriteandmusic.unity3d";//??
    private int version = 0;
    [SerializeField] AudioSource source;
    [SerializeField] SpriteRenderer sRenderer;
    public void OnClickDownload()
    {
        StartCoroutine(DownloadAndCache());
    }

    IEnumerator DownloadAndCache()
    {
        while (!Caching.ready)
            yield return null;

        var www = WWW.LoadFromCacheOrDownload(bundleURL, version);
        yield return www;

        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log(www.error);
            yield break;
        }
        Debug.Log("Загрузка успешна");
        var assetBundle = www.assetBundle;
        string musicName = "cat_1_minute_sound.mp3";
        string modelName = "berry.png";

        var musicRequest = assetBundle.LoadAssetAsync(musicName, typeof(AudioClip));
        yield return musicRequest;
        Debug.Log("музыкальный файл распакован");

        var modelRequest = assetBundle.LoadAssetAsync(modelName, typeof(Sprite));
        yield return modelRequest;
        Debug.Log("модель распакована");

        source.clip = musicRequest.asset as AudioClip;
        source.Play();
        sRenderer.sprite = modelRequest.asset as Sprite;
    }

    IEnumerator GetText()
    {
        using (UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(bundleURL))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                // Get downloaded asset bundle
                AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(uwr);
                var loadAsset = bundle.LoadAssetAsync<GameObject>("Assets/Players/MainPlayer.prefab");
                yield return loadAsset;

                Instantiate(loadAsset.asset);
            }
        }
    }
}
