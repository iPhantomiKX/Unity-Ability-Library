using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private float previousFrameTime;
    private Attack attack;

    public PlayerAttackingState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine)
    {
        attack = stateMachine.m_Attacks[attackIndex];
    }

    public override void Enter()
    {
        stateMachine.m_Animator.CrossFadeInFixedTime(attack.m_AnimationName, attack.m_TransitionDuration);
    }

    public override void Tick(float deltaTime)
    {
        //This works but idk why
        //Prevents Press and Hold Behaviour!
        if (stateMachine.m_InputReader.IsAttacking)
        {
            if (stateMachine.m_CurrentAttackTimer >= stateMachine.m_MaxAttackTimer)
            {
                stateMachine.m_InputReader.IsAttacking = false;
                stateMachine.m_CurrentAttackTimer = 0f;
            }
            else
                stateMachine.m_CurrentAttackTimer += deltaTime;
        }
        else
            stateMachine.m_CurrentAttackTimer = 0f;

        //let the player still experience forces when attacking
        Move(deltaTime);
        FaceTarget();

        float normalizedTime = GetNormalizedTime();

        if (normalizedTime >= previousFrameTime && normalizedTime < 1f)
        {
            //if player is trying to attack
            if (stateMachine.m_InputReader.IsAttacking)
            {
                TryComboAttack(normalizedTime);
            }
        }
        else
        {
            if (stateMachine.m_Targeter.m_CurrentTarget != null)
            {
                stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            }
            else
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            }
        }

        previousFrameTime = normalizedTime;
    }

    public override void Exit()
    {
    }

    private void TryComboAttack(float normalizedTime)
    {
        //if we don't have a combo (make sure we have a combo attack)
        if (attack.m_ComboStateIndex == -1) { return; }

        //if we can combo (make sure we are far enough to do it)
        if (normalizedTime < attack.m_ComboAttackTime) { return; }

        //if we are, switch state to attack
        stateMachine.SwitchState(new PlayerAttackingState(stateMachine, attack.m_ComboStateIndex));
    }

    private float GetNormalizedTime()
    {
        AnimatorStateInfo currentInfo = stateMachine.m_Animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = stateMachine.m_Animator.GetNextAnimatorStateInfo(0);

        if (stateMachine.m_Animator.IsInTransition(0) && nextInfo.IsTag("Attack"))
        {
            return nextInfo.normalizedTime;
        }
        else if (!stateMachine.m_Animator.IsInTransition(0) && currentInfo.IsTag("Attack"))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }

    }
}
