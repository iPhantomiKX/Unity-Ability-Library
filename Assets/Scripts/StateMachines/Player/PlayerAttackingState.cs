using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class PlayerAttackingState : PlayerBaseState
{
    private Attack m_Attack;
    public int m_AttackIndex = 0;
    private bool m_AlreadyAppliedForce = false;

    public override void Enter()
    {
        if (m_AttackIndex >= ((PlayerStateMachine)m_StateMachine).m_Attacks.Length - 1)
            m_AttackIndex = 0;
        else
            ++m_AttackIndex;
        m_Attack = ((PlayerStateMachine)m_StateMachine).m_Attacks[m_AttackIndex];
        ((PlayerStateMachine)m_StateMachine).m_CurrentAnimNormalizedTime = 0f;
        ((PlayerStateMachine)m_StateMachine).m_InputReader.AttackEvent += OnAttack;
        ((PlayerStateMachine)m_StateMachine).m_Weapon.SetDamage(m_Attack.m_Damage);
        ((PlayerStateMachine)m_StateMachine).m_Animator.CrossFadeInFixedTime(m_Attack.m_AnimationName, m_Attack.m_TransitionDuration);
        ((PlayerStateMachine)m_StateMachine).m_MaxAttackTimer = m_Attack.m_TransitionDuration;
    }

    public override void Tick(float deltaTime)
    {
        ((PlayerStateMachine)m_StateMachine).m_CurrentAnimNormalizedTime += deltaTime;

        TryApplyForce(((PlayerStateMachine)m_StateMachine).transform.forward, 
                      m_Attack.m_Force, 
                      deltaTime, 
                      ref m_AlreadyAppliedForce);

        if (((PlayerStateMachine)m_StateMachine).m_CurrentAnimNormalizedTime >= m_Attack.m_ComboAttackTime)
        {
            ((PlayerStateMachine)m_StateMachine).m_CurrentAnimNormalizedTime = m_Attack.m_ComboAttackTime;
            ((PlayerStateMachine)m_StateMachine).m_CurrentAttackTimer += deltaTime;

            if(((PlayerStateMachine)m_StateMachine).m_CurrentAttackTimer >= ((PlayerStateMachine)m_StateMachine).m_MaxAttackTimer)
            {
                ((PlayerStateMachine)m_StateMachine).m_CurrentAttackTimer = 0f;
                ((PlayerStateMachine)m_StateMachine).m_CurrentAnimNormalizedTime = 0f;
                if (((PlayerStateMachine)m_StateMachine).m_Targeter.m_CurrentTarget != null)
                {
                    ((PlayerStateMachine)m_StateMachine).m_NextStateName = "Targeting";
                    ((PlayerStateMachine)m_StateMachine).SwitchState(((PlayerStateMachine)m_StateMachine).GetPlayerStateFromName(((PlayerStateMachine)m_StateMachine).m_NextStateName), ((PlayerStateMachine)m_StateMachine));
                }
                else
                {
                    ((PlayerStateMachine)m_StateMachine).m_NextStateName = "FreeLooking";
                    ((PlayerStateMachine)m_StateMachine).SwitchState(((PlayerStateMachine)m_StateMachine).GetPlayerStateFromName(((PlayerStateMachine)m_StateMachine).m_NextStateName), ((PlayerStateMachine)m_StateMachine));
                }
            }
        }
    }

    public override void Exit()
    {
        ((PlayerStateMachine)m_StateMachine).m_InputReader.AttackEvent -= OnAttack;
    }

    private void OnAttack()
    {
        if(((PlayerStateMachine)m_StateMachine).m_CurrentAnimNormalizedTime >= m_Attack.m_ComboAttackTime)
        {
            ((PlayerStateMachine)m_StateMachine).m_CurrentAttackTimer = 0f;
            ((PlayerStateMachine)m_StateMachine).m_CurrentAnimNormalizedTime = 0f;
            ((PlayerStateMachine)m_StateMachine).m_NextStateName = "Attacking";
            ((PlayerStateMachine)m_StateMachine).SwitchState(((PlayerStateMachine)m_StateMachine).GetPlayerStateFromName(((PlayerStateMachine)m_StateMachine).m_NextStateName), ((PlayerStateMachine)m_StateMachine));
        }
    }
}
