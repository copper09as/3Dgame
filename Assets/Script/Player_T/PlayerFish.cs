using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFish : PlayerState
{
    public PlayerFish(PlayerStateMachine stateMachine, Animator animator, Rigidbody rb)
    {
        this.stateMachine = stateMachine;
        this.animator = animator;
        this.rb = rb;
        transform = stateMachine.player.transform;
    }
    private void FishOver()
    {
        stateMachine.TransState(playerState.Idle);
    }

    public override void Enter()
    {
        animator.SetBool("isFishing", true);
        _ = GameApp.Instance.uiManager.GetUi("FishUiPanel");
        GameApp.Instance.eventCenter.AddNormalListener("FishOver", FishOver);
    }

    public override void Exit()
    {
        animator.SetBool("isFishing", false);
        GameApp.Instance.eventCenter.RemoveNormalListener("FishOver",FishOver);
    }
    public override void Update()
    {
        return;
    }

}
