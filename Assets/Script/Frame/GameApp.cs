using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameApp : MonoSingleTon<GameApp>
{
    
    public UiManager uiManager;
    public AudioManager audioManager;
    public ResManager resManager;
    protected override void Awake()
    {
        base.Awake();
        uiManager = new UiManager();
        audioManager = new AudioManager();
        resManager = new ResManager();

    }
    private void Update()
    {
        NetManager.Update();
    }
    private void Start()
    {
        //_ = uiManager.GetUi("LoginPanel");
        _ = uiManager.GetUi("TipPanel", "STTART Fish");
    }

}
