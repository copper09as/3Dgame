using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TipPanel : UiBase,IinitUi
{
    [SerializeField] private Button closeBtn;
    public TextMeshProUGUI tip;
    public override void OnEnter()
    {
        base.OnEnter();
        closeBtn.onClick.AddListener(OnCloseClick);
    }
    public void OnCloseClick()
    {
        OnExit();
    }
    public override void OnExit()
    {
        base.OnExit();
        Destroy(gameObject);
        closeBtn.onClick.RemoveListener(OnCloseClick);

    }

    public void InitId(int id)
    {
        throw new System.NotImplementedException();
    }
}
