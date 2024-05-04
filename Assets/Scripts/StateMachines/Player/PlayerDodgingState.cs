using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgingState : PlayerBaseState
{
    public readonly int m_DodgeHash = Animator.StringToHash("Dodge");

    [Header("Dodge Settings")]
    public float m_Force = 10.0f;
    public bool m_AlreadyAppliedForce = false;

    public override void Enter()
    {
        stateMachine.m_Animator.CrossFadeInFixedTime(m_DodgeHash, stateMachine.m_CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        TryApplyForce(stateMachine.transform.forward,
                      m_Force,
                      deltaTime,
                      ref m_AlreadyAppliedForce);

        if (stateMachine.m_ForceReceiver.GetImpact().sqrMagnitude <= 0.1f)
        {
            if(stateMachine.m_Targeter.m_CurrentTarget != null)
            {
                stateMachine.m_NextStateName = "Targeting";
                stateMachine.SwitchState(stateMachine.GetPlayerStateFromName(stateMachine.m_NextStateName));
            }
            else
            {
                stateMachine.m_NextStateName = "FreeLooking";
                stateMachine.SwitchState(stateMachine.GetPlayerStateFromName(stateMachine.m_NextStateName));
            }
        }
    }

    public override void Exit()
    {
        m_AlreadyAppliedForce = false;
    }
}
