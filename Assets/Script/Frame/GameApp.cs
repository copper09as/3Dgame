using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.RestService;
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
    public TaskManager taskManager;
    public PlayerData playerData;
    
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        uiManager = new UiManager();
        audioManager = new AudioManager();
        resManager = new ResManager();
        inventoryManager = new InventoryManager(56);
        eventCenter = new EventCenter();
        taskManager = new TaskManager();
        inventoryManager.AddItem(1, 1);
        inventoryManager.AddItem(2, 1);
        inventoryManager.AddItem(3,1);
        //LoadData();

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
        if(Input.GetKeyDown(KeyCode.B))
        {
            _ = uiManager.GetUi("BagPanel");
        }        //NetManager.Update();
        if (Input.GetKeyDown(KeyCode.P))
        {
            _ = uiManager.GetUi("AddPanel");
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            _ = uiManager.GetUi("TaskPanel");
        }


    }    private void Start()
    {
        //NetManager.AddEventListener(NetEvent.Close, OnConnectClose);
        //NetManager.AddMsgListener("MsgKick", OnMsgKick);
        //_ = uiManager.GetUi("LoginPanel");
         //_ = uiManager.GetUi("BagPanel");
        //_ = uiManager.GetUi("TipPanel", "Be Kick");
        //inventoryManager.RemoveItem(2, 2);
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
