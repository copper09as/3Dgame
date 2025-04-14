using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;

public class EventCenter
{
    private Dictionary<string, Func<bool>> boolListener;
    private Dictionary<string, Action> normalListener;
    public EventCenter ()
    {
        boolListener = new Dictionary<string, Func<bool>>();
        normalListener = new Dictionary<string, Action>();
    }
    public void AddNormalListener(string name,Action action)
    {
        if(normalListener.ContainsKey(name))
        {
            normalListener[name] += action;
        }
        else
        {
            normalListener[name] = action;
        }
    }
    public bool RemoveNormalListener(string name,Action action)
    {
        if (!normalListener.ContainsKey(name))
        {
            return false;
        }
        else
        {
            normalListener[name] -= action;
            if (normalListener[name]==null)
            {
                normalListener.Remove(name);
                return true;
            }
            return true;
        }
    }
    public bool TrigNormalListener(string name)
    {
        Action value;
        if(normalListener.TryGetValue(name,out value))
        {
            value?.Invoke();
            return true;
        }
        return false;
    }
}
