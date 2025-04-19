using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddPanel : UiBase
{
    [SerializeField] private Button Add_1;
    [SerializeField] private Button Add_2;
    [SerializeField] private List<AddSlot> slots;
    [SerializeField] private List<ItemId> items;

    public override void OnEnter()
    {
        base.OnEnter();
        Add_1.onClick.AddListener(AddOne);
        Add_2.onClick.AddListener(AddSec);
        UpdateSlot();
    }
    public override void OnExit()
    {
        base.OnExit();
        Add_1.onClick.RemoveListener(AddOne);
        Add_2.onClick.RemoveListener(AddSec);
    }
    private void AddSec()
    {
        if (GameApp.Instance.inventoryManager.ItemCount(1) >= 1 &&
            GameApp.Instance.inventoryManager.ItemCount(2) >= 1 &&
            GameApp.Instance.inventoryManager.ItemCount(3) >= 1)
        {
            GameApp.Instance.inventoryManager.RemoveItem(1, 1);
            GameApp.Instance.inventoryManager.RemoveItem(2, 1);
            GameApp.Instance.inventoryManager.RemoveItem(3, 1);
            GameApp.Instance.inventoryManager.AddItem(4, 1);
        }
    }
    private void AddOne()
    {
        if (GameApp.Instance.inventoryManager.ItemCount(1) >= 1 &&
            GameApp.Instance.inventoryManager.ItemCount(2) >= 1 &&
            GameApp.Instance.inventoryManager.ItemCount(3) >= 1)
        {
            GameApp.Instance.inventoryManager.RemoveItem(1, 1);
            GameApp.Instance.inventoryManager.RemoveItem(2, 1);
            GameApp.Instance.inventoryManager.RemoveItem(3, 1);
            GameApp.Instance.inventoryManager.AddItem(4, 1);
        }

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
