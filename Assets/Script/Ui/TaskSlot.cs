using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class TaskSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI taskName;
    [SerializeField] private TextMeshProUGUI taskDes;
    public void SetSlot(TaskData taskData)
    {
        taskName.text = taskData.name;
        if (taskData.isOver)
        {
            taskDes.text = "TaskFinish";
            return;
        }
        else
        {
            taskDes.text = taskData.description;
        }
    }
}
