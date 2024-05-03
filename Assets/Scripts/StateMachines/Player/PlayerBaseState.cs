using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected void Move(Vector3 motion, float deltaTime)
    {
        PlayerStateMachine.Instance.m_Controller.Move((motion + PlayerStateMachine.Instance.m_ForceReceiver.m_Movement) * deltaTime);
    }

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void FaceTarget()
    {
        if (PlayerStateMachine.Instance.m_Targeter.m_CurrentTarget == null) return;

        Vector3 lookPos = PlayerStateMachine.Instance.m_Targeter.m_CurrentTarget.transform.position - PlayerStateMachine.Instance.transform.position;
        lookPos.y = 0f;

        PlayerStateMachine.Instance.transform.rotation = Quaternion.LookRotation(lookPos);
    }

    protected void TryApplyForce(Vector3 direction, float force, float deltaTime, ref bool hasAlreadyAppliedForce)
    {
        if (!hasAlreadyAppliedForce)
        {
            PlayerStateMachine.Instance.m_ForceReceiver.AddForce(direction * force);
            hasAlreadyAppliedForce = true;
        }
        Move(PlayerStateMachine.Instance.m_ForceReceiver.GetImpact(), deltaTime);
    }
}