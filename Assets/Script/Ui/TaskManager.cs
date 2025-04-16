using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager
{
    public List<TaskData> taskDatas;
    public TaskManager()
    {
        taskDatas = new List<TaskData>();
    }
    public void AddTask(TaskData task)
    {
        taskDatas.Add(task);
        GameApp.Instance.eventCenter.TrigNormalListener("UpdateTaskUi");
    }
    public void FinishTask(string name)
    {
        var i = taskDatas.FindIndex(i=>i.name == name);
        taskDatas[i].isOver = true;
        GameApp.Instance.eventCenter.TrigNormalListener("UpdateTaskUi");
    }
}
[System.Serializable]
public class TaskData
{
    public string name;
    public string description;
    public bool isOver;
    public void Finish() => isOver = true;
}