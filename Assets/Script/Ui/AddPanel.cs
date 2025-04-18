using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddPanel : UiBase
{
    [SerializeField] private Button Add_1;
    [SerializeField] private Button Add_2;
    [SerializeField] private Button Add_3;
    [SerializeField] private List<AddSlot> slots;
    [SerializeField] private List<ItemId> items;

    public override void OnEnter() 
    {
        base.OnEnter();
        Add_1.onClick.AddListener(AddOne);
        UpdateSlot();
    }
    public override void OnExit()
    {
        base.OnExit();
        Add_1.onClick.RemoveListener(AddOne);
    }
    private void AddOne()
    {
        if(GameApp.Instance.inventoryManager.ItemCount(1)>=1 && 
            GameApp.Instance.inventoryManager.ItemCount(2) >= 1 &&
            GameApp.Instance.inventoryManager.ItemCount(3) >= 1)
        {
            GameApp.Instance.inventoryManager.RemoveItem(1, 1);
            GameApp.Instance.inventoryManager.RemoveItem(2, 1);
            GameApp.Instance.inventoryManager.RemoveItem(3, 1);
            GameApp.Instance.inventoryManager.AddItem(4, 1);
        }
        //GameApp.Instance.eventCenter.TrigNormalListener("UpdateUi");
    }
    private void UpdateSlot()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            var slot = slots[i];
            slot.SetSlot(GameApp.Instance.inventoryManager.FindItem(items[i].id), items[i].mount);
        }
    }
}
