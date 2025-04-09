using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ObjectPool<T> where T : MonoBehaviour
{
    private MyStack<T> currentObStack;
    private HashSet<T> allOb;
    private int count = 0;
    private string obName;
    private string fold;
    private Transform transform;
    public ObjectPool(string obName, string fold, int InitCap = 0, Transform transform = null)
    {
        currentObStack = new MyStack<T>(InitCap);
        allOb = new HashSet<T>();
        this.obName = obName;
        this.fold = fold;
        this.transform = transform;
    }
    public T Get()
    {
        T ob = null;

        if (currentObStack.Count() > 0)
        {
            ob = currentObStack.Pop();
        }
        else
        {
            ob = Instantiate();
        }

        count++;
        ob.gameObject.SetActive(true);
        return ob;
    }
    public void Destroy(T ob)
    {
        count--;
        currentObStack.Push(ob);
        ob.gameObject.SetActive(false);
    }

    public void ClearAll()
    {
        foreach (T ob in allOb)
        {
            if (ob != null && ob.gameObject != null)
            {
                Addressables.ReleaseInstance(ob.gameObject);
            }
        }
        allOb.Clear();
        currentObStack.Clear();
        count = 0;
    }
    private T Instantiate()
    {
        var handle = Addressables.InstantiateAsync("Assets/Prefab/" + fold + "/" + obName);

        handle.WaitForCompletion();

        var obj = handle.Result;

        obj.transform.SetParent(transform);

        allOb.Add(obj.GetComponent<T>());

        return obj.GetComponent<T>();
    }
}
