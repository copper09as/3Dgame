using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskPanel : UiBase
{
    public List<TaskSlot> slots;
    public List<TaskData> data;
    GameObject slotskinRes;
    [SerializeField]Transform slotsTransform;
    public override void OnOpen()
    {
        data = GameApp.Instance.taskManager.taskDatas;
        slots = new List<TaskSlot>();
        slotskinRes = GameApp.Instance.resManager.LoadPrefab("Assets/Prefab/Ui/TaskSlot");
        base.OnOpen();
    }
    public override void OnEnter()
    {
        base.OnEnter();
        GameApp.Instance.eventCenter.AddNormalListener("UpdateTaskUi", UpdateTask);
        UpdateTask();
    }
    public override void OnExit()
    {
        base.OnExit();
        GameApp.Instance.eventCenter.RemoveNormalListener("UpdateTaskUi", UpdateTask);
    }
    public void UpdateTask()
    {
        while (slots.Count!=data.Count)
        {
            var slotOb = (GameObject)Instantiate(slotskinRes);
            slotOb.transform.SetParent(slotsTransform);
            var slot = slotOb.GetComponent<TaskSlot>();
            slots.Add(slot);
        }
        for(int i = 0;i<data.Count;i++)
        {
            slots[i].SetSlot(data[i]);
        }
    }

}
