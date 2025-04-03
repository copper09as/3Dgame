using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public string name;
    public string fold;
    private ObjectPool<Squre> testPool;
    private List<Squre> testList = new List<Squre>(); 
    private void Awake()
    {
        testPool = new ObjectPool<Squre>(name, fold);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            testList.Add(testPool.Get());
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (testList.Count>0)
            {
                testPool.Destroy(testList[0]);
                testList.RemoveAt(0);
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            testList.Clear();
            testPool.ClearAll();
        }

    }
}
