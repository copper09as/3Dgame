using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragPanel : UiBase,IinitUi,IcloseUi
{ 
    [SerializeField] private Image image;
    [SerializeField]private int lastId = -1;
    [SerializeField]private int currentId = -1;

    public override void OnEnter()
    {
        base.OnEnter();
        this.raycasting = false;
    }
    public override void OnExit()
    {
        //lastSlot.SetSlot(currentData, currentMount, lastId);
        //currentSlot.SetSlot(lastData, lastMount, currentId);
        if(lastId != -1 && currentId != -1)
        {
            GameApp.Instance.inventoryManager.SwapItem(lastId, currentId);
            GameApp.Instance.eventCenter.TrigNormalListener("UpdateUi");
            lastId = -1;
            currentId = -1;
        }
        base.OnExit();
    }
    
    private void Update()
    {
        this.transform.position = Input.mousePosition;
    }

    public void InitId(int id)
    {
        this.lastId = id;
    }

    public void closeId(int id)
    {
        this.currentId = id;
    }
}
