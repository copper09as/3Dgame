using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ItemDataListSo",menuName = "Item/ItemDataListSo")]
public class ItemDataList : ScriptableObject
{
    public List<ItemData> itemDatas;
}
