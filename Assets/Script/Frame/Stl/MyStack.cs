using System.Collections.Generic;
using UnityEditor.Build.Pipeline;
using UnityEngine;

public class MyStack<T>
{
    private List<T> priList;
    private int count;
    public int Count()
    {
        foreach (var i in priList)
        {
            Debug.LogWarning(i);
        }
        return count;
    }
    public MyStack(int InitCap = 0)
    {
        priList = new List<T>(InitCap);
        count = 0;
    }
    public void Push(T ob)
    {
        priList.Add(ob);
        count++;
    }
    public T Pop()
    {
        var temp = priList[count - 1];
        priList.RemoveAt(count - 1);
        count--;
        return temp;
    }
    public T Peek()
    {

        return priList[count - 1];
    }
    public bool Contains(T ob)
    {

        return priList.Contains(ob);
    }
    public bool SetLast(T ob)
    {
        if (Count() == 1)
            return false;
        if (ob.Equals(Peek()))
            return false;
        Debug.Log(ob);
        priList.Remove(ob);
        Push(ob);
        Debug.LogWarning(Peek());
        return true;
    }
    public T Find(int index)
    {
        return (T)priList[index];
    }
    public void Clear()
    {
        priList.Clear();

        count = 0;
    }
    public bool Remove(T ob)
    {
        if(priList.Contains(ob))
        {
            count -= 1;
            priList.Remove(ob);
            return true;
        }
        return false;

    }
}
