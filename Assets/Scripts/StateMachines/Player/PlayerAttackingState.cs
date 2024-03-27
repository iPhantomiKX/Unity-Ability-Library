using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private Attack attack;

    public PlayerAttackingState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine)
    {
        attack = stateMachine.m_Attacks[attackIndex];
        stateMachine.m_CurrentAnimNormalizedTime = 0f;
    }

    public override void Enter()
    {
        stateMachine.m_InputReader.AttackEvent += OnAttack;
        stateMachine.m_Animator.CrossFadeInFixedTime(attack.m_AnimationName, attack.m_TransitionDuration);
        stateMachine.m_MaxAttackTimer = attack.m_TransitionDuration;
    }

    public override void Tick(float deltaTime)
    {
        stateMachine.m_CurrentAnimNormalizedTime += deltaTime;

        if(stateMachine.m_CurrentAnimNormalizedTime >= attack.m_ComboAttackTime)
        {
            stateMachine.m_CurrentAnimNormalizedTime = attack.m_ComboAttackTime;
            stateMachine.m_CurrentAttackTimer += deltaTime;
            if(stateMachine.m_CurrentAttackTimer >= stateMachine.m_MaxAttackTimer)
            {
                stateMachine.m_CurrentAttackTimer = 0f;
                stateMachine.m_CurrentAnimNormalizedTime = 0f;
                if (stateMachine.m_Targeter.m_CurrentTarget != null)
                {
                    stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
                }
                else
                {
                    stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
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
        if(stateMachine.m_CurrentAnimNormalizedTime >= attack.m_ComboAttackTime)
        {
            stateMachine.m_CurrentAttackTimer = 0f;
            stateMachine.m_CurrentAnimNormalizedTime = 0f;
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, attack.m_ComboStateIndex));
        }
    }
}
