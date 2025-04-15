using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ResManager
{
    public GameObject LoadPrefab(string path)
    {
        var handle = Addressables.LoadAssetAsync<GameObject>(path);

        handle.WaitForCompletion();

        var obj = handle.Result;

        return obj;
    }
    public T LoadData<T>(string path)
    {
        var handle = Addressables.LoadAssetAsync<T>(path);

        handle.WaitForCompletion();

        var obj = handle.Result;

        return obj;
    }
}
