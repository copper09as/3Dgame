using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameApp : MonoSingleTon<GameApp>
{
    public static string id = "";
    public UiManager uiManager;
    public AudioManager audioManager;
    public ResManager resManager;
    public InventoryManager inventoryManager;
    public EventCenter eventCenter;
    public GameData gameData;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        uiManager = new UiManager();
        audioManager = new AudioManager();
        resManager = new ResManager();
        inventoryManager = new InventoryManager(56);
        eventCenter = new EventCenter();
        LoadData();

    }
    private void LoadData()
    {
        var gameData = GameSave.LoadByJson<GameData>("GameData.json");
        if (gameData!=null)
        {
            inventoryManager.items = gameData.items;
        }
    }
    private void Update()
    {
        //NetManager.Update();
        if(Input.GetKeyDown(KeyCode.A))
        {
            _ = uiManager.GetUi("BagPanel");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            _ = uiManager.GetUi("LoginPanel");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            _ = uiManager.GetUi("RegisterPanel");
        }
    }    private void Start()
    {
<<<<<<< Updated upstream
        //_ = uiManager.GetUi("LoginPanel");
        _ = uiManager.GetUi("TipPanel", "STTART Fish");
=======
        //NetManager.AddEventListener(NetEvent.Close, OnConnectClose);
        //NetManager.AddMsgListener("MsgKick", OnMsgKick);
        //_ = uiManager.GetUi("LoginPanel");
         //_ = uiManager.GetUi("BagPanel");
        //_ = uiManager.GetUi("TipPanel", "Be Kick");
        inventoryManager.RemoveItem(2, 2);
>>>>>>> Stashed changes
    }

    private void OnMsgKick(MsgBase msgBase)
    {
        _ = uiManager.GetTipUi("Be Kick");
    }

    private void OnConnectClose(string err)
    {
        Debug.Log("¶Ï¿ªÁ¬½Ó");
    }
}
