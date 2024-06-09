using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCastAbilityState : PlayerBaseState
{
    public float m_Runtime = 0.0f;  //Adjustable

    public override void Enter()
    {
    }

    public override void Tick(float deltaTime)
    {
        //Call Ability Update Here
        //stateMachine.PlayerCurrentAbility.Update(deltaTime);
        if (m_Runtime >= 1.0f)
        {
            Debug.LogError("Player is back to FreeLookState");
            stateMachine.SwitchState(stateMachine.GetPlayerStateFromName(stateMachine.m_NextStateName));
        }
        else
            m_Runtime += deltaTime;
    }

    public override void Exit()
    {
        //reset the timer
        m_Runtime = 0.0f;
    }
}
