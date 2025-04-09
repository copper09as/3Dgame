using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    private PlayerState currentState;
    private PlayerState lastState;
    public readonly PlayerController player;
    private readonly PlayerStateFactory stateFactory;
    public PlayerStateMachine(PlayerController player,Animator animator,Rigidbody rb)
    {
        this.player = player;
        stateFactory = new PlayerStateFactory(this, animator,rb);
    }
    
    public void ReturnState()
    {
        if(NullState())
        {
            return;
        }
        currentState.Exit();
        lastState.Enter();
        currentState = lastState;
    }
    public void TransState(playerState state)
    {
        PlayerState obState = Create(state);
        if(obState == currentState)
        {
            return;
        }
        if(currentState!=null)
        {
            lastState = currentState;
            currentState.Exit();
        }
        currentState = obState;
        obState.Enter();
    }
    public void Init(playerState state) => TransState(state);
    private bool NullState() => currentState == null || lastState == null;
    public PlayerState Create(playerState state)=> stateFactory.CreateState(state);
    public void Update()=> currentState.Update();

}
