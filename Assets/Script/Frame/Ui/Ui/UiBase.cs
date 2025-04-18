using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public abstract class UiBase : MonoBehaviour, ICanvasRaycastFilter
{
    protected bool raycasting = false;
    public bool isActive = false;
    public virtual void Awake()
    {

    }
    public virtual void OnEnter()
    {
        raycasting = true;
        gameObject.SetActive(true);
        isActive = true;
        Debug.Log(gameObject.name + "Enter");
    }
    public virtual void OnExit()
    {
        raycasting = false;
        isActive = false;
        gameObject.SetActive(false);
        Debug.Log(gameObject.name + "Exit");
    }
    public virtual void OnOpen()
    {
        Debug.Log(gameObject.name);
        gameObject.SetActive(true);
        Debug.Log(gameObject.name + "Open");
        OnEnter();
    }
    public virtual void OnClose()
    {
        OnExit();
    }
    public virtual void Init()
    {
        transform.SetParent(GameObject.Find("Canvas").transform);
        transform.localPosition = Vector3.zero;
    }
    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        return raycasting;
    }
}
