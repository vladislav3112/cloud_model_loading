using System;
using UnityEngine;
using System.Collections;

public class LoadAssetBundleAnimation : MonoBehaviour 
{
    public string BundleURL = "ftp://89.208.87.29/model.unity3d";
    public string AssetName;
    public int version = 0;

    void Start()
    {
        StartCoroutine(DownloadAndCache());
    }

    IEnumerator DownloadAndCache()
    {
        // Wait for the Caching system to be ready
        while (!Caching.ready)
            yield return null;

        // Load the AssetBundle file from Cache if it exists with the same version or download and store it in the cache
        using (WWW www = WWW.LoadFromCacheOrDownload(BundleURL, version))
        {
            yield return www;
            if (www.error != null)
                throw new Exception("WWW download had an error:" + www.error);
            AssetBundle bundle = www.assetBundle;
            if (AssetName == "")
                throw new Exception("empty asset bundle name:" + www.error);
            else
            {
                var prefab = bundle.LoadAsset<GameObject>("cat_Jump");
                Instantiate(prefab);
            }// Unload the AssetBundles compressed contents to conserve memory
            bundle.Unload(false);

        } // memory is freed from the web stream (www.Dispose() gets called implicitly)
    }
}