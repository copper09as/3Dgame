using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TalkPanel : MonoBehaviour,IPointerClickHandler
{
    public TalkSo talk;
    public TextMeshProUGUI text;
    public bool CanActive = true;
    public List<GameObject> hideOb;
    int times = 0;
    private void OnEnable()
    {
        times = 0;
        text.text = talk.talk[times];
        foreach(var ob in hideOb)
        {
            ob.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (times < talk.talk.Count - 1)
            {
                times += 1;
            }
            if(times == talk.talk.Count-1)
            {
                foreach (var ob in hideOb)
                {
                    ob.SetActive(true);
                }
            }
            text.text = talk.talk[times];
        }
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if (times > 0)
            {
                times -= 1;
            }
            text.text = talk.talk[times];
        }
    }
    public void Task1()
    {
        TaskData task1 = new TaskData()
        {
            name = "Hello hhy",
            description = "Fish!",
            isOver = false
        };
        GameApp.Instance.taskManager.AddTask(task1);
        CanActive = false;
        gameObject.SetActive(false);
    }
    public void Task2()
    {
        TaskData task2 = new TaskData()
        {
            name = "Hello mskj",
            description = "Fly",
            isOver = false
        };
        GameApp.Instance.taskManager.AddTask(task2);
        gameObject.SetActive(false);
    }
    public void Task3()
    {
        TaskData task3 = new TaskData()
        {
            name = "Hello ljh",
            description = "dead",
            isOver = false
        };
        GameApp.Instance.taskManager.AddTask(task3);
        gameObject.SetActive(false);
    }
}
