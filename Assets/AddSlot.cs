using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddSlot : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI needCount;
    public void SetSlot(ItemData item, int count)
    {
        image.sprite = item.image;
        needCount.text = count.ToString();
    }
}
