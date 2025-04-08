using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerWalk : PlayerState
{
    public PlayerWalk(PlayerStateMachine stateMachine,Animator animator)
    {
        this.stateMachine = stateMachine;
        this.animator = animator;
        transform = stateMachine.player.transform; 
    }
    public override void Enter()
    {
        animator.SetBool("IsWalk", true);
    }

    public override void Exit()
    {
        animator.SetBool("IsWalk", false);
    }

    public override void Update()
    {
        base.Update();
        if (magnitude<0.1f)
        {
            stateMachine.TransState(new PlayerIdle(stateMachine,animator));
        }
        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            stateMachine.TransState(new PlayerRun(stateMachine,animator));
        }
    }
}
public class PlayerRun : PlayerState
{
    public PlayerRun(PlayerStateMachine stateMachine,Animator animator)
    {
        this.stateMachine = stateMachine;
        this.animator = animator;
        transform = stateMachine.player.transform;
    }
    public override void Enter()
    {
        moveSpeed = 10f;
        animator.SetBool("IsRunning", true);
    }

    public override void Exit()
    {
        moveSpeed = 5f;
        animator.SetBool("IsRunning",false);
    }
    public override void Update()
    {
        base.Update();
        if (magnitude < 0.1f)
        {
            stateMachine.TransState(new PlayerIdle(stateMachine,animator));
        }
    }
    }

