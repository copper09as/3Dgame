using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerWalk : PlayerState
{
    public PlayerWalk(PlayerStateMachine stateMachine,Animator animator,Rigidbody rb)
    {
        this.stateMachine = stateMachine;
        this.animator = animator;
        this.rb = rb;
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
            stateMachine.TransState(playerState.Idle);
        }
        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            stateMachine.TransState(playerState.Run);
        }
    }
}
public class PlayerRun : PlayerState
{
    public PlayerRun(PlayerStateMachine stateMachine,Animator animator,Rigidbody rb)
    {
        this.stateMachine = stateMachine;
        this.animator = animator;
        this.rb = rb;
        transform = stateMachine.player.transform;
    }
    public override void Enter()
    {
        moveSpeed = GameApp.Instance.playerData.runSpeed;
        GameApp.Instance.playerData.turnSpeed = 0.09f;
        animator.SetBool("IsRunning", true);
    }

    public override void Exit()
    {
        moveSpeed = GameApp.Instance.playerData.moveSpeed;
        GameApp.Instance.playerData.turnSpeed = 0.04f;
        animator.SetBool("IsRunning",false);
    }
    public override void Update()
    {
        base.Update();
        if (magnitude < 0.1f)
        {
            stateMachine.TransState(playerState.Idle);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            stateMachine.TransState(playerState.Walk);
        }
    }
    }

