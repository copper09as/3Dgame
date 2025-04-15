using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class UiManager
{
    List<UiBase> uiStack = new List<UiBase>();
    Dictionary<string, UiBase> UiDic = new Dictionary<string, UiBase>();

    /// <summary>
    /// 进入指定ui，无法生成多个相同ui
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<UiBase> GetUi(string name)
    {
        UiBase ui = null;
        if (!UiDic.ContainsKey(name))
        {
            var handle = Addressables.InstantiateAsync("Assets/Prefab/Ui/" + name);
            await handle.Task;
            ui = handle.Result.GetComponent<UiBase>();
            UiDic[name] = ui;
            ui.Init();
        }
        else
        {
            ui = UiDic[name];
        }
        if(ui.isActive)
        {
            Debug.LogWarning("Un active " + ui);
            return ui;
        }
        if (ui != null)
        {
            if (uiStack.Contains(ui))
            {
                uiStack.Remove(ui);
                uiStack.Add(ui);
                uiStack[uiStack.Count - 2].OnExit();
                ui.OnEnter();//第二次进入是enter
            }
            else
            {
                if (uiStack.Count > 0)
                {
                    uiStack[uiStack.Count-1].OnExit();
                }
                uiStack.Add(ui);
                ui.OnOpen();//第一次进入是Open
            }
        }
        return ui;
    }
    /// <summary>
    /// 可生成多个相同ui，不入缓存区，用于生成提示
    /// </summary>
    /// <param name="name"></param>
    /// <param name="tip"></param>
    /// <returns></returns>
    public async Task<TipPanel> GetTipUi(string tip = "")
    {
        TipPanel ui = null;
        var handle = Addressables.InstantiateAsync("Assets/Prefab/Ui/TipPanel");
        await handle.Task;
        ui = handle.Result.GetComponent<TipPanel>();
        ui.Init();
        ui.OnOpen();
        ui.GetComponent<TipPanel>().tip.text = tip;
        return ui;
    }
    /// <summary>
    /// 只生成一个ui，且需要额外的初始化以及导入数据和文本，不会影响上级ui
    /// </summary>
    /// <param name="name"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public async Task<UiBase> GetUi(string name, int id)
    {
        UiBase ui = null;

        if (!UiDic.ContainsKey(name))
        {
            var handle = Addressables.InstantiateAsync("Assets/Prefab/Ui/" + name);
            await handle.Task;
            ui = handle.Result.GetComponent<UiBase>();
            UiDic[name] = ui;
            ui.Init();
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
                uiStack.Remove(ui);
                uiStack.Add(ui);
                uiStack[uiStack.Count - 2].OnExit();
                ui.OnEnter();//第二次进入是enter
            }
            else
            {
                ui.OnOpen();//第一次进入是Open
            }
            IinitUi iinit = ui.GetComponent<IinitUi>();
            iinit.InitId(id);
        }
        return ui;
    }
    /// <summary>
    /// 返回上一级ui
    /// </summary>
    /*public void PopUi()
    {
        if (uiStack.Count() > 0)
        {
            uiStack.Pop().OnExit();
            if (uiStack.Count() > 0)
            {
                uiStack.Peek().OnEnter();
            }
        }
        //Debug.Log(uiStack.Count());
    }*/

    public bool CloseUi(string name, int id = -2)
    {
        UiBase value;
        if (UiDic.TryGetValue(name, out value))
        {
            if (id != -2)
            {
                var close = value.GetComponent<IcloseUi>();
                if (close != null)
                    close.closeId(id);
            }
            else
            {
                uiStack.Remove(value);
                if (uiStack.Count > 0)
                {
                    uiStack[uiStack.Count-1].OnEnter();
                }
            }
            value.OnExit();
            return true;
        }
        return false;
    }
    /// <summary>
    /// 关闭所有ui
    /// </summary>
    private void CloseAll()
    {
        while (uiStack.Count > 0)
        {
            //uiStack.Pop().OnClose();
        }
    }
    private UiBase CreateUi(string name)
    {
        UiBase ui = null;
        if (!UiDic.ContainsKey(name))
        {
            var handle = Addressables.InstantiateAsync("Assets/Prefab/Ui/" + name);

            handle.WaitForCompletion();

            ui = handle.Result.GetComponent<UiBase>();

            UiDic[name] = ui;
            ui.Init();
        }
        else
        {
            ui = UiDic[name];
        }
        return ui;
    }
}
