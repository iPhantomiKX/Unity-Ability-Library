using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine stateMachine;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachine.m_Controller.Move((motion + stateMachine.m_ForceReceiver.m_Movement) * deltaTime);
    }

    protected void FaceTarget()
    {
        if (stateMachine.m_Targeter.m_CurrentTarget == null) return;

        Vector3 lookPos = stateMachine.m_Targeter.m_CurrentTarget.transform.position - stateMachine.transform.position;
        lookPos.y = 0f;

        stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
    }
}