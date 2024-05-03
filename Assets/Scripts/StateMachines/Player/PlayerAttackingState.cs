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
        if (m_AttackIndex >= PlayerStateMachine.Instance.m_Attacks.Length - 1)
            m_AttackIndex = 0;
        else
            ++m_AttackIndex;
        m_Attack = PlayerStateMachine.Instance.m_Attacks[m_AttackIndex];
        PlayerStateMachine.Instance.m_CurrentAnimNormalizedTime = 0f;
        PlayerStateMachine.Instance.m_InputReader.AttackEvent += OnAttack;
        PlayerStateMachine.Instance.m_Weapon.SetDamage(m_Attack.m_Damage);
        PlayerStateMachine.Instance.m_Animator.CrossFadeInFixedTime(m_Attack.m_AnimationName, m_Attack.m_TransitionDuration);
        PlayerStateMachine.Instance.m_MaxAttackTimer = m_Attack.m_TransitionDuration;
    }

    public override void Tick(float deltaTime)
    {
        PlayerStateMachine.Instance.m_CurrentAnimNormalizedTime += deltaTime;

        TryApplyForce(PlayerStateMachine.Instance.transform.forward, 
                      m_Attack.m_Force, 
                      deltaTime, 
                      ref m_AlreadyAppliedForce);

        if (PlayerStateMachine.Instance.m_CurrentAnimNormalizedTime >= m_Attack.m_ComboAttackTime)
        {
            PlayerStateMachine.Instance.m_CurrentAnimNormalizedTime = m_Attack.m_ComboAttackTime;
            PlayerStateMachine.Instance.m_CurrentAttackTimer += deltaTime;

            if(PlayerStateMachine.Instance.m_CurrentAttackTimer >= PlayerStateMachine.Instance.m_MaxAttackTimer)
            {
                PlayerStateMachine.Instance.m_CurrentAttackTimer = 0f;
                PlayerStateMachine.Instance.m_CurrentAnimNormalizedTime = 0f;
                m_AttackIndex = -1;
                if (PlayerStateMachine.Instance.m_Targeter.m_CurrentTarget != null)
                {
                    PlayerStateMachine.Instance.m_NextStateName = "Targeting";
                    PlayerStateMachine.Instance.SwitchState(PlayerStateMachine.Instance.GetPlayerStateFromName(PlayerStateMachine.Instance.m_NextStateName));
                }
                else
                {
                    PlayerStateMachine.Instance.m_NextStateName = "FreeLooking";
                    PlayerStateMachine.Instance.SwitchState(PlayerStateMachine.Instance.GetPlayerStateFromName(PlayerStateMachine.Instance.m_NextStateName));
                }
            }
        }
    }

    public override void Exit()
    {
        PlayerStateMachine.Instance.m_InputReader.AttackEvent -= OnAttack;
    }

    private void OnAttack()
    {
        if(PlayerStateMachine.Instance.m_CurrentAnimNormalizedTime >= m_Attack.m_ComboAttackTime)
        {
            PlayerStateMachine.Instance.m_CurrentAttackTimer = 0f;
            PlayerStateMachine.Instance.m_CurrentAnimNormalizedTime = 0f;
            PlayerStateMachine.Instance.m_NextStateName = "Attacking";
            PlayerStateMachine.Instance.SwitchState(PlayerStateMachine.Instance.GetPlayerStateFromName(PlayerStateMachine.Instance.m_NextStateName));
        }
    }
}
