using System.Collections.Generic;

public class MyStack<T>
{
    private List<T> priList;
    private int count;
    public int Count()
    {
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
        priList.Remove(ob);
        count -= 1;
        Push(ob);
        return true;
    }
    public T Find(int index)
    {
        return (T)priList[index];
    }
}
