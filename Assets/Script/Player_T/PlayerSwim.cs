using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerSwim : PlayerState
{
    public PlayerSwim(PlayerStateMachine stateMachine, Animator animator, Rigidbody rb)
    {
        this.stateMachine = stateMachine;
        this.animator = animator;
        this.rb = rb;
        transform = stateMachine.player.transform;
    }
    public override void Enter()
    {
        animator.SetBool("IsSwimming", true);
        Physics.gravity = new Vector3(0,0,0);
    }

    public override void Exit()
    {
        animator.SetBool("IsSwimming", false);
        Physics.gravity = new Vector3(0, -GameApp.Instance.playerData.Gravity, 0);
    }
    public override void Update()
    {
        base.Update();
        if(!stateMachine.player.IsInWater())
        {
            stateMachine.ReturnState();
        }
    }

}
