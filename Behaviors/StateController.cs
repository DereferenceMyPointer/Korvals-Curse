using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * State controller
 * Has a current state and sets new states as returned by the current state's tick function
 */
public class StateController : MonoBehaviour
{
    public State currentState;

    private void Awake()
    {
        currentState.Begin();
    }

    // Tick current state and update based on the result
    private void Update()
    {
        State s = currentState.Tick();
        if(s != currentState)
        {
            SetState(s);
        }
    }

    // Set the state and trigger necessary end and begin statements
    public void SetState(State newState)
    {
        currentState.End();
        currentState = newState;
        currentState.Begin();
    }

}
