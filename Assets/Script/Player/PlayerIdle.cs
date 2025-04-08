using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : PlayerState
{
    public PlayerIdle(PlayerStateMachine stateMachine,Animator animator)
    {
        this.stateMachine = stateMachine;
        this.animator = animator;
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
            stateMachine.TransState(new PlayerWalk(stateMachine,animator));
        }
    }
}
