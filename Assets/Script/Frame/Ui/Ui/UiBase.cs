using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiBase : MonoBehaviour, ICanvasRaycastFilter
{
    protected bool raycasting = false;
    public void OnEnter()
    {
        gameObject.transform.localScale = Vector3.one;
        raycasting = true;
        Debug.Log(gameObject.name + "Enter");
    }
    public void OnExit()
    {
        gameObject.transform.localScale = Vector3.zero;//≤‚ ‘œ‘ æ
        raycasting = false;
        Debug.Log(gameObject.name + "Exit");
    }
    public void OnOpen()
    {
        gameObject.SetActive(true);
        Debug.Log(gameObject.name + "Open");
        OnEnter();
    }
    public void OnClose()
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
