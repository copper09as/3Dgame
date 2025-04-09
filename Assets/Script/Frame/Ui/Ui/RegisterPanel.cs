using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterPanel : UiBase
{
    [SerializeField] private InputField idInput;
    [SerializeField] private InputField pwInput;
    [SerializeField] private InputField repInput;
    [SerializeField] private Button regBtn;
    [SerializeField] private Button closeBtn;
    public override void OnEnter()
    {
        base.OnEnter();
        regBtn.onClick.AddListener(OnRegClick);
        closeBtn.onClick.AddListener(OnCloseClick);
        NetManager.AddMsgListener("MsgRegister", OnMsgRegister);
    }
    public override void OnExit()
    {
        base.OnExit();
        idInput.text = string.Empty;
        pwInput.text = string.Empty;
        repInput.text = string.Empty;
        regBtn.onClick.RemoveListener(OnRegClick);
        closeBtn.onClick.RemoveListener(OnCloseClick);
        NetManager.RemoveListenear("MsgRegister", OnMsgRegister);
    }
    private void OnMsgRegister(MsgBase msgBase)
    {
        MsgRegister msg = (MsgRegister)msgBase;
        if (msg.result==0)
        {
            Debug.Log("×¢²á³É¹¦");
            GameApp.Instance.uiManager.PopUi();
            _ = GameApp.Instance.uiManager.GetUi("TipPanel", "Reg Succ!");
            
        }
        else
        {
            _ = GameApp.Instance.uiManager.GetUi("TipPanel", "Reg Fail!");
        }

    }

    private void OnCloseClick()
    {
        GameApp.Instance.uiManager.PopUi();
    }

    private void OnRegClick()
    {
        if (idInput.text == "" || pwInput.text == "")
        {
            _ = GameApp.Instance.uiManager.GetUi("TipPanel", "pw or id is empty! ");
            return;
        }
        if(pwInput.text != repInput.text)
        {
            _ = GameApp.Instance.uiManager.GetUi("TipPanel", "pw shoule equal repw! ");
            return;
        }
        MsgRegister msgReg = new MsgRegister();
        msgReg.id = idInput.text;
        msgReg.pw = pwInput.text;
        NetManager.Send(msgReg);
    }
}
