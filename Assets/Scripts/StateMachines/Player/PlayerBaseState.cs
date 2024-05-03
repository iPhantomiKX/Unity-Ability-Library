using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State
{

    //public PlayerBaseState(PlayerStateMachine m_StateMachine)
    //{
    //    this.m_StateMachine = m_StateMachine;
    //}

    protected void Move(Vector3 motion, float deltaTime)
    {
        ((PlayerStateMachine)m_StateMachine).m_Controller.Move((motion + ((PlayerStateMachine)m_StateMachine).m_ForceReceiver.m_Movement) * deltaTime);
    }

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void FaceTarget()
    {
        if (((PlayerStateMachine)m_StateMachine).m_Targeter.m_CurrentTarget == null) return;

        Vector3 lookPos = ((PlayerStateMachine)m_StateMachine).m_Targeter.m_CurrentTarget.transform.position - ((PlayerStateMachine)m_StateMachine).transform.position;
        lookPos.y = 0f;

        ((PlayerStateMachine)m_StateMachine).transform.rotation = Quaternion.LookRotation(lookPos);
    }

    protected void TryApplyForce(Vector3 direction, float force, float deltaTime, ref bool hasAlreadyAppliedForce)
    {
        if (!hasAlreadyAppliedForce)
        {
            ((PlayerStateMachine)m_StateMachine).m_ForceReceiver.AddForce(direction * force);
            hasAlreadyAppliedForce = true;
        }
        Move(((PlayerStateMachine)m_StateMachine).m_ForceReceiver.GetImpact(), deltaTime);
    }
}