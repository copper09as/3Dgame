using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateFactory
{
    private Dictionary<playerState, PlayerState> stateDic = new Dictionary<playerState, PlayerState>();

    private readonly PlayerStateMachine stateMachine;

    private readonly Animator animator;

    private readonly Rigidbody rb;
    public PlayerStateFactory(PlayerStateMachine stateMachine, Animator animator,Rigidbody rb)
    {
        this.stateMachine = stateMachine;
        this.animator = animator;
        this.rb = rb;
    }
    public PlayerState CreateState(playerState state)
    {
        PlayerState createState = null;
        if(stateDic.ContainsKey(state))
        {
            return stateDic[state];
        }
        else
        {
            switch (state)
            {
                case playerState.Idle:
                    createState = new PlayerIdle(stateMachine,animator,rb);break;
                case playerState.Jump:
                    createState = new PlayerJump(stateMachine, animator,rb);break;
                case playerState.Run:
                    createState = new PlayerRun (stateMachine, animator,rb);break;
                case playerState.Walk:
                    createState = new PlayerWalk(stateMachine, animator,rb);break;
                case playerState.Fly:
                    createState = new PlayerFly(stateMachine, animator, rb); break;
                case playerState.Fish:
                    createState = new PlayerFish(stateMachine, animator, rb);break;
            }
            if (createState != null)
                stateDic[state] = createState;
            return createState;
        }

    }
}
