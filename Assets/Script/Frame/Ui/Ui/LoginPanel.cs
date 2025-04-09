using System;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : UiBase
{
    [SerializeField] private InputField idInput;
    [SerializeField] private InputField pwInput;
    [SerializeField] private Button loginBtm;
    [SerializeField] private Button regBtn;
    public override void OnOpen()
    {
        base.OnOpen();
        loginBtm.onClick.AddListener(OnLoginClick);
        regBtn.onClick.AddListener(OnRegClick);
        NetManager.AddMsgListener("MsgLogin", OnMsgLogin);
        NetManager.AddEventListener(NetEvent.ConnectSucc, OnConnectSucc);
        NetManager.AddEventListener(NetEvent.ConnectFail, OnConnectFail);
        NetManager.Connect("60.204.187.12", 80);
    }

    private void OnConnectFail(string err)
    {
        Debug.Log("OnConnectFail");
    }

    private void OnConnectSucc(string err)
    {
        _ = GameApp.Instance.uiManager.GetUi("TipPanel",err);
    }

    private void OnMsgLogin(MsgBase msgBase)
    {
        MsgLogin msg = (MsgLogin)msgBase;
        if (msg.result==0)
        {
            Debug.Log("µÇÂ½³É¹¦");
            _ = GameApp.Instance.uiManager.GetUi("TipPanel", "Log Succ");
        }
        else
            _ = GameApp.Instance.uiManager.GetUi("TipPanel", "Log Fail");

    }


    private void OnRegClick()
    {
        _ = GameApp.Instance.uiManager.GetUi("RegisterPanel");
    }

    private void OnLoginClick()
    {
        if(idInput.text == "" || pwInput.text == "")
        {
            _ = GameApp.Instance.uiManager.GetUi("TipPanel", "pw or id is empty! ");
            return;
        }
        MsgLogin msgLogin = new MsgLogin();
        msgLogin.id = idInput.text;
        msgLogin.pw = pwInput.text;
        NetManager.Send(msgLogin);
    }
}
