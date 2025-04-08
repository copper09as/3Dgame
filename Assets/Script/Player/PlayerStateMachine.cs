using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    private PlayerState currentState;
    public readonly PlayerController player;
    private PlayerState lastState;
    public PlayerStateMachine(PlayerController player)
    {
        this.player = player;
    }
    public void Init(PlayerState initState)
    {
        currentState = initState;
        currentState.Enter();
    }
    public void ReturnState()
    {
        if(currentState==null || lastState == null)
        {
            Debug.Log("return fail");
            return;
        }
        currentState.Exit();
        lastState.Enter();
        currentState = lastState;
    }

    public void TransState(PlayerState obState)
    {
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
    public void Update()
    {
        if (currentState == null)
            return;
        currentState.Update();
    }

}
