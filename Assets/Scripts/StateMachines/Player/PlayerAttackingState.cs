using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class PlayerAttackingState : PlayerBaseState
{
    [SerializeField] private int m_AttackIndex = -1;
    private Attack m_Attack;
    private bool m_AlreadyAppliedForce = false;

    public override void Enter()
    {
        if (m_AttackIndex >= stateMachine.m_Attacks.Length - 1)
            m_AttackIndex = 0;
        else
            ++m_AttackIndex;
        m_Attack = stateMachine.m_Attacks[m_AttackIndex];
        stateMachine.m_CurrentAnimNormalizedTime = 0f;
        stateMachine.m_InputReader.AttackEvent += OnAttack;
        stateMachine.m_Weapon.SetDamage(m_Attack.m_Damage);
        stateMachine.m_Animator.CrossFadeInFixedTime(m_Attack.m_AnimationName, m_Attack.m_TransitionDuration);
        stateMachine.m_MaxAttackTimer = m_Attack.m_TransitionDuration;
    }

    public override void Tick(float deltaTime)
    {
        stateMachine.m_CurrentAnimNormalizedTime += deltaTime;

        TryApplyForce(stateMachine.transform.forward, 
                      m_Attack.m_Force, 
                      deltaTime, 
                      ref m_AlreadyAppliedForce);

        if (stateMachine.m_CurrentAnimNormalizedTime >= m_Attack.m_ComboAttackTime)
        {
            stateMachine.m_CurrentAnimNormalizedTime = m_Attack.m_ComboAttackTime;
            stateMachine.m_CurrentAttackTimer += deltaTime;

            if(stateMachine.m_CurrentAttackTimer >= stateMachine.m_MaxAttackTimer)
            {
                stateMachine.m_CurrentAttackTimer = 0f;
                stateMachine.m_CurrentAnimNormalizedTime = 0f;
                m_AttackIndex = -1;
                if (stateMachine.m_Targeter.m_CurrentTarget != null)
                {
                    stateMachine.m_NextStateName = "Targeting";
                }
                else
                {
                    stateMachine.m_NextStateName = "FreeLooking";
                }
            }
        }
    }

    public override void Exit()
    {
        stateMachine.m_InputReader.AttackEvent -= OnAttack;
    }

    private void OnAttack()
    {
        if(stateMachine.m_CurrentAnimNormalizedTime >= m_Attack.m_ComboAttackTime)
        {
            stateMachine.m_CurrentAttackTimer = 0f;
            stateMachine.m_CurrentAnimNormalizedTime = 0f;
            stateMachine.m_NextStateName = "Attacking";
        }
    }
}
