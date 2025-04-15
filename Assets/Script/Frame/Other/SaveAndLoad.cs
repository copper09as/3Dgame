using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameSave
{
    public static void SaveByJson(string saveFilename, object data)
    {
        var json = JsonUtility.ToJson(data);
        var path = Path.Combine(Application.persistentDataPath, saveFilename);
        try
        {
            File.WriteAllText(path, json);
            Debug.Log($"成功存储在{path}");
        }
        catch (System.Exception exception)
        {
            Debug.Log($"在{path}存储失败\n {exception}");
        }
    }
    public static T LoadByJson<T>(string saveFilename)
    {
        var path = Path.Combine(Application.persistentDataPath, saveFilename);

        try
        {
            var json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<T>(json);
            Debug.Log($"成功在{path}读取");
            return data;
        }
        catch (System.Exception exception)
        {
            Debug.Log($"在{path}读取失败\n {exception}");
            return default;
        }
    }

    public static void DeleteFile(string saveFilename)
    {
        var path = Path.Combine(Application.persistentDataPath, saveFilename);
        try { File.Delete(path); }
        catch (System.Exception exception)
        {
            Debug.Log($"在{path}删除失败\n {exception}");
        }
    }
}
