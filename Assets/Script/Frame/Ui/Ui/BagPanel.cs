using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Loading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagPanel : UiBase
{
    [SerializeField] private List<BagSlot> slots;
    [SerializeField]private ItemId[] items;
    [SerializeField] int perMount = 25;
    [SerializeField] Transform slotsTransform;
    [SerializeField] int maxPage;
    [SerializeField] int currentPage = 1;
    [SerializeField] Button nextPageBtn;
    [SerializeField] Button prePageBtn;
    [SerializeField] Button closeBtn;
    [SerializeField] TextMeshProUGUI pageText;
    InventoryManager inventoryManager;
    int times = 0;
    bool inEnter = false;
    public void NextPage()
    { 
        if(currentPage<maxPage)
        {
            currentPage += 1;
        }
        UpdateSlot();
    }
    public void LastPage()
    {
        if(currentPage>1)
        {
            currentPage -= 1;
        }
        UpdateSlot();
    }
    private void OnCloseClick()
    {
        GameApp.Instance.uiManager.CloseUi("BagPanel");
        BagSlot.inDrag = false;
    }
    public override void Awake()
    {
        inventoryManager = GameApp.Instance.inventoryManager;
        items = inventoryManager.GetItems();
        maxPage = items.Length / perMount;
        CreateSlot();
    }

    public override void OnOpen()
    {
        base.OnOpen();
    }
    public override void OnEnter()
    {
        currentPage = 1;
        nextPageBtn.onClick.AddListener(NextPage);
        prePageBtn.onClick.AddListener(LastPage);
        closeBtn.onClick.AddListener(OnCloseClick);
        GameApp.Instance.eventCenter.AddNormalListener("UpdateUi", UpdateSlot);
        UpdateSlot();
        inEnter = true;
        base.OnEnter();
    }
    public override void OnExit()
    {
        
        nextPageBtn.onClick.RemoveListener(NextPage);
        prePageBtn.onClick.RemoveListener(LastPage);
        closeBtn.onClick.RemoveListener(OnCloseClick);
        GameApp.Instance.eventCenter.RemoveNormalListener("UpdateUi", UpdateSlot);
        BagSlot.inDrag = false;
        foreach(var slot in slots)
        {
            slot.Clear();
        }
        GameApp.Instance.uiManager.CloseUi("BagTipPanel");
        GameApp.Instance.uiManager.CloseUi("ItemDragPanel");
        inEnter = false;
        base.OnExit();
    }
    private void CreateSlot()
    {
        GameObject slotskinRes = GameApp.Instance.resManager.LoadPrefab("Assets/Prefab/Ui/Slot");
        for (int i = perMount * (currentPage - 1); i < perMount * currentPage; i++)
        {
            var slotOb = (GameObject)Instantiate(slotskinRes);
            slotOb.transform.SetParent(slotsTransform);
            var slot = slotOb.GetComponent<BagSlot>();
            slots.Add(slot);
            slot.SetSlot(inventoryManager.FindItem(items[i].id), items[i].mount,i);
        }
        
    }
    private void UpdateSlot()
    {
        for (int i = perMount * (currentPage - 1); i < perMount * currentPage; i++)
        {
            var slot = slots[i%perMount];
            slot.SetSlot(inventoryManager.FindItem(items[i].id), items[i].mount,i);
        }
        pageText.text = currentPage.ToString() + "/" + maxPage.ToString();
    }

}
