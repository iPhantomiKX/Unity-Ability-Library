using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    private State currentState;

    public void SwitchState(State newState, StateMachine stateMachine)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.m_StateMachine = stateMachine;
        currentState?.Enter();
    }

    public virtual void Update()
    {
        currentState?.Tick(Time.deltaTime);
    }
}
