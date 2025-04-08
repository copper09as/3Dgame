using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TipPanel : UiBase
{
    [SerializeField] private Button CloseBtn;
    public TextMeshProUGUI tip;
    public override void OnOpen()
    {
        base.OnOpen();
        CloseBtn.onClick.AddListener(OnCloseClick);
    }
    public void OnCloseClick()
    {
        OnExit();
        GameApp.Instance.uiManager.PopUi();
    }
    public override void OnExit()
    {
        base.OnExit();
        CloseBtn.onClick.RemoveListener(OnCloseClick);

    }
}
