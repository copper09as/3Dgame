using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : PlayerState
{
    public PlayerIdle(PlayerStateMachine stateMachine,Animator animator,Rigidbody rb)
    {
        this.stateMachine = stateMachine;
        this.animator = animator;
        this.rb = rb;
        transform = stateMachine.player.transform;
    }
    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        base.Update();

        if (magnitude>=0.1f)
        {
            stateMachine.TransState(playerState.Walk);
        }
        if(Input.GetKeyDown(KeyCode.Delete) && stateMachine.player.IsFish())
        {
            stateMachine.TransState(playerState.Fish);
        }
    }
}
