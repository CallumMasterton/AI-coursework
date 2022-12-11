using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager
{
    State currState;

    public void ChangeState(State newState)//Cheacks when the AI enters or leaves a state
    {
        if (currState != null)
        {
            currState.Exit();
        }
        currState = newState;
        newState.Enter();
    }

    public void Update()
    {
        if (currState != null) currState.Execute();//Updates the AI
    }
}
