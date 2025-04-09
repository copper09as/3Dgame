using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiBase : MonoBehaviour, ICanvasRaycastFilter
{
    protected bool raycasting = false;
    public virtual void OnEnter()
    {
        raycasting = true;
        gameObject.SetActive(true);
        Debug.Log(gameObject.name + "Enter");
    }
    public virtual void OnExit()
    {
        raycasting = false;
        gameObject.SetActive(false);
        Debug.Log(gameObject.name + "Exit");
    }
    public virtual void OnOpen()
    {
        gameObject.SetActive(true);
        Debug.Log(gameObject.name + "Open");
        OnEnter();
    }
    public virtual void OnClose()
    {
        gameObject.SetActive(false);
        Debug.Log(gameObject.name + "Close");
        OnExit();
    }

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        return raycasting;
    }
}
