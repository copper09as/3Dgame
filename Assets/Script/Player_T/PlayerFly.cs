using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFly : PlayerState
{
    public PlayerFly(PlayerStateMachine stateMachine, Animator animator, Rigidbody rb)
    {
        this.stateMachine = stateMachine;
        this.animator = animator;
        this.rb = rb;
        transform = stateMachine.player.transform;
    }
    public override void Enter()
    {
        Physics.gravity = new Vector3(0, -100f, 0);
        stateMachine.player.GetBird().SetActive(true);
        animator.SetBool("IsFlying", true);
    }

    public override void Exit()
    {
        Physics.gravity = new Vector3(0, -500f, 0);
        stateMachine.player.GetBird().SetActive(false);
        animator.SetBool("IsFlying", false);
    }
    public override void Update()
    {
        base.Update();
        if(stateMachine.player.IsGrounded() || Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.TransState(playerState.Idle);
        }
    }

}
