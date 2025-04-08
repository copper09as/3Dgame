using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class UiManager
{
    MyStack<UiBase> uiStack = new MyStack<UiBase>();
    Dictionary<string, UiBase> UiDic = new Dictionary<string, UiBase>();

    /// <summary>
    /// 进入指定ui
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<UiBase> GetUi(string name,string tip = "")
    {
        UiBase ui = null;

        if (!UiDic.ContainsKey(name))
        {
            var handle = Addressables.InstantiateAsync("Assets/Prefab/Ui/" + name);
            await handle.Task;
            ui = handle.Result.GetComponent<UiBase>();
            UiDic[name] = ui;
            handle.Result.transform.SetParent(GameObject.Find("Canvas").transform);
            handle.Result.transform.localPosition = Vector3.zero;
        }
        else
        {
            ui = UiDic[name];
            Debug.Log("ui即将进入");
        }
        if (ui != null)
        {
            if (uiStack.Contains(ui))
            {
                if(uiStack.SetLast(ui))
                {
                    uiStack.Find(uiStack.Count() - 2).OnExit();
                    ui.OnEnter();
                }
            }
            else
            {
                if (uiStack.Count() > 0)
                {
                    uiStack.Peek().OnExit();
                }
                uiStack.Push(ui);
                ui.OnOpen();
            }
        }
        if(tip!="")
        {
            ui.GetComponent<TipPanel>().tip.text = tip;
        }
        Debug.Log(uiStack.Count());
        Debug.Log(uiStack.Contains(ui));
        return ui;
    }
    /// <summary>
    /// 返回上一级ui
    /// </summary>
    public void PopUi()
    {
        if(uiStack.Count()>0)
        {
            uiStack.Pop().OnExit();
            if(uiStack.Count() > 0)
            {
                uiStack.Peek().OnEnter();
            }
        }
    }
    /// <summary>
    /// 关闭所有ui
    /// </summary>
    private void CloseAll()
    {
        while(uiStack.Count()>0)
        {
            uiStack.Pop().OnClose();
        }
    }
}
