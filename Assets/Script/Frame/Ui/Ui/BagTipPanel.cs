using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BagTipPanel : UiBase,IinitUi
{   
    [SerializeField] private ItemData item;
    [SerializeField] private TextMeshProUGUI desText;

    public void InitId(int id)
    {
        var item = GameApp.Instance.inventoryManager.FindItem(id);
        desText.text = item.description;
    }

}
