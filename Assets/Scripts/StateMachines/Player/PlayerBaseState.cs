using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine stateMachine
    {
        get
        {
            return PlayerStateMachine.Instance;
        }
    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachine.m_Controller.Move((motion + stateMachine.m_ForceReceiver.m_Movement) * deltaTime);
    }

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void TryApplyForce(Vector3 direction, float force, float deltaTime, ref bool hasAlreadyAppliedForce)
    {
        if (!hasAlreadyAppliedForce)
        {
            stateMachine.m_ForceReceiver.AddForce(direction * force);
            hasAlreadyAppliedForce = true;
        }
        Move(stateMachine.m_ForceReceiver.GetImpact(), deltaTime);
    }
}