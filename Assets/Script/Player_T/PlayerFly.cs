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
        stateMachine.player.GetBird().SetActive(true);
        animator.SetBool("IsFlying", true);
    }

    public override void Exit()
    {
        stateMachine.player.GetBird().SetActive(false);
        animator.SetBool("IsFlying", false);
    }
    public override void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");

        float vertical = Input.GetAxis("Vertical");
        Physics.gravity = new Vector3(0, -GameApp.Instance.playerData.flyGravity, 0);
        Vector3 direction = stateMachine.player.transform.forward * vertical;
        direction += stateMachine.player.transform.right * horizontal;
        direction.Normalize();
        magnitude = direction.magnitude;
        direction *= moveSpeed*3f;
        direction.y = 0f;
        stateMachine.player.Move(direction);
       
        if (stateMachine.player.IsInWater())
        {
            stateMachine.TransState(playerState.Idle);
            stateMachine.TransState(playerState.Swim);
        }
        if (stateMachine.player.IsGrounded() || Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.TransState(playerState.Idle);
        }
    }

}
