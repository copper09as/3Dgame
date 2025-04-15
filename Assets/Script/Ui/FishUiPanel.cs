using UnityEngine;

public class FishUiPanel : UiBase
{
    [SerializeField] private SliderBar powBar;
    [SerializeField] private SliderBar rotBar;
    [SerializeField]float powProcess;
    [SerializeField]float rotProcess;
    private ItemDataList fishDataList;
    [SerializeField]private int fishSpeed;
    FishState state = FishState.pow;
    public override void OnOpen()
    {
        base.OnOpen();
    }
    public override void OnEnter()
    {
        base.OnEnter();
        fishSpeed = Random.Range(1, 5);
        powProcess = 0f;
        rotProcess = 0f;
        state = FishState.pow;
    }
    private void OnPow()
    {
        if (state != FishState.pow)
            return;
        if (powProcess < 1f)
        {
            powProcess += Time.deltaTime*fishSpeed;
        }
        else
        {
            powProcess = 0f;
        }
        powBar.UpdateSliderBar(1f, powProcess);
    }
    private void OnRot()
    {
        if (state != FishState.rot)
            return;
        if (rotProcess < 1f)
        {
            rotProcess += Time.deltaTime * fishSpeed;
        }
        else
        {
            rotProcess = 0f;
        }
        rotBar.UpdateSliderBar(1f, rotProcess);
    }
    private void Update()
    {
        OnPow();
        OnRot();
        if(Input.GetMouseButtonDown(0))
        {
            if(state != FishState.over)
            {
                state += 1;
                if(state == FishState.over)
                {
                    OnOver();
                }
            }
        }
    }
    private void OnOver()
    {
        float rotScore = Mathf.Abs(rotProcess - 0.5f);
        float powScore = Mathf.Abs(powProcess - 0.5f);
        int score = (int)(fishSpeed * 100) - (int)Mathf.Abs(rotScore * 100) - (int)Mathf.Abs(powScore * 100);
        if (rotScore<0.07f)
            if(powScore < 0.07f)
            {
                _ = GameApp.Instance.uiManager.GetTipUi("FishSucc" + "Score:" + score.ToString());
                return;
            }
        _ = GameApp.Instance.uiManager.GetTipUi("FishFail" + score.ToString());
        GameApp.Instance.uiManager.CloseUi("FishUiPanel");


    }
    private enum FishState
    {
        pow,
        rot,
        over
    }

}
