using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagSlot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler,IPointerClickHandler
{
    [SerializeField] private ItemData item;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI mountText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private int id;
    [SerializeField] private int mount;
    [SerializeField] bool inChose = false;

    public static bool inDrag = false;
    public void OnPointerDown(PointerEventData eventData)
    {

        
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(item.id == -1 || inDrag)
        {
            return;
        }
        _ = GameApp.Instance.uiManager.GetUi("BagTipPanel", item.id);
        inChose = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item.id == -1)
        {
            return;
        }
        GameApp.Instance.uiManager.CloseUi("BagTipPanel",-3);
        inChose = false;
    }



    public void SetSlot(ItemData item,int mount,int id)
    { 
        this.item = item;
        this.image.sprite = item.image;
        this.nameText.text = item.name;
        this.mountText.text = mount.ToString();
        this.mount = mount;
        this.id = id;
    }
    public void Clear()
    {
        inChose = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (item.id == -1 && !inDrag)
            {
                return;
            }
            if (inDrag)
            {
                inDrag = false;
                GameApp.Instance.uiManager.CloseUi("ItemDragPanel", id);
            }
            else
            {
                inDrag = true;
                _ = GameApp.Instance.uiManager.GetUi("ItemDragPanel", id);
            }
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            GameApp.Instance.eventCenter.TrigNormalListener(item.id.ToString());
            Debug.Log(item.id.ToString());
        }
    }
}
