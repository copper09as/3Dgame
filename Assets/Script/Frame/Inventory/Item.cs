using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
[CreateAssetMenu(fileName = "ItemDataSo",menuName = "Item/ItemDataSo")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite image;
    public bool isStack;
    public ItemEffect effect;
    public bool canSold;
    public int price;
    public int id;
    [TextArea]
    public string description;
}
[System.Serializable]
public struct ItemId
{
    public int id;
    public int mount;
}

