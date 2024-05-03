using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    private readonly int m_TargetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");
    private readonly int m_TargetingForwardHash = Animator.StringToHash("TargetingForward");
    private readonly int m_TargetingRightHash = Animator.StringToHash("TargetingRight");

    public override void Enter()
    {
        ((PlayerStateMachine)m_StateMachine).m_InputReader.TargetEvent += OnCancel;
        ((PlayerStateMachine)m_StateMachine).m_InputReader.AttackEvent += OnAttack;
        ((PlayerStateMachine)m_StateMachine).m_Animator.CrossFadeInFixedTime(m_TargetingBlendTreeHash, ((PlayerStateMachine)m_StateMachine).m_CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if(((PlayerStateMachine)m_StateMachine).m_InputReader.IsAttacking)
        {
            ((PlayerStateMachine)m_StateMachine).m_NextStateName = "Attacking";
            ((PlayerStateMachine)m_StateMachine).SwitchState(((PlayerStateMachine)m_StateMachine).GetPlayerStateFromName(((PlayerStateMachine)m_StateMachine).m_NextStateName), ((PlayerStateMachine)m_StateMachine));
            return;
        }
        if(((PlayerStateMachine)m_StateMachine).m_Targeter.m_CurrentTarget == null)
        {
            ((PlayerStateMachine)m_StateMachine).m_IsFocusingEnemy = false;
            ((PlayerStateMachine)m_StateMachine).m_NextStateName = "FreeLooking";
            ((PlayerStateMachine)m_StateMachine).SwitchState(((PlayerStateMachine)m_StateMachine).GetPlayerStateFromName(((PlayerStateMachine)m_StateMachine).m_NextStateName), ((PlayerStateMachine)m_StateMachine));
            return;
        }

        Vector3 movement = CalculateMovement();

        Move(movement * ((PlayerStateMachine)m_StateMachine).m_TargetingMovementSpeed, deltaTime);

        UpdateAnimator(deltaTime);

        FaceTarget();
    }

    private void UpdateAnimator(float deltaTime)
    {
        if(((PlayerStateMachine)m_StateMachine).m_InputReader.MovementValue.y == 0)
        {
            ((PlayerStateMachine)m_StateMachine).m_Animator.SetFloat(m_TargetingForwardHash, 0, 0.1f, deltaTime);
        }
        else
        {
            float value = ((PlayerStateMachine)m_StateMachine).m_InputReader.MovementValue.y > 0 ? 1f : -1f;
            ((PlayerStateMachine)m_StateMachine).m_Animator.SetFloat(m_TargetingForwardHash, value, 0.1f, deltaTime);
        }

        if (((PlayerStateMachine)m_StateMachine).m_InputReader.MovementValue.x == 0)
        {
            ((PlayerStateMachine)m_StateMachine).m_Animator.SetFloat(m_TargetingRightHash, 0, 0.1f, deltaTime);
        }
        else
        {
            float value = ((PlayerStateMachine)m_StateMachine).m_InputReader.MovementValue.x > 0 ? 1f : -1f;
            ((PlayerStateMachine)m_StateMachine).m_Animator.SetFloat(m_TargetingRightHash, value, 0.1f, deltaTime);
        }
    }

    public override void Exit()
    {
        ((PlayerStateMachine)m_StateMachine).m_IsFocusingEnemy = false;
        ((PlayerStateMachine)m_StateMachine).m_InputReader.TargetEvent -= OnCancel;
        ((PlayerStateMachine)m_StateMachine).m_InputReader.AttackEvent -= OnAttack;
    }

    private void OnAttack()
    {
        ((PlayerStateMachine)m_StateMachine).m_NextStateName = "Attacking";
        ((PlayerStateMachine)m_StateMachine).SwitchState(((PlayerStateMachine)m_StateMachine).GetPlayerStateFromName(((PlayerStateMachine)m_StateMachine).m_NextStateName), ((PlayerStateMachine)m_StateMachine));
    }

    private void OnCancel()
    {
        ((PlayerStateMachine)m_StateMachine).m_Targeter.Cancel();

        if(((PlayerStateMachine)m_StateMachine).m_IsFocusingEnemy)
        {
            ((PlayerStateMachine)m_StateMachine).m_IsFocusingEnemy = false;
            ((PlayerStateMachine)m_StateMachine).m_NextStateName = "FreeLooking";
            ((PlayerStateMachine)m_StateMachine).SwitchState(((PlayerStateMachine)m_StateMachine).GetPlayerStateFromName(((PlayerStateMachine)m_StateMachine).m_NextStateName), ((PlayerStateMachine)m_StateMachine));
        }
    }

    private Vector3 CalculateMovement()
    {
        Vector3 movement = new Vector3();

        movement += ((PlayerStateMachine)m_StateMachine).transform.right * ((PlayerStateMachine)m_StateMachine).m_InputReader.MovementValue.x;
        movement += ((PlayerStateMachine)m_StateMachine).transform.forward * ((PlayerStateMachine)m_StateMachine).m_InputReader.MovementValue.y;

        return movement;
    }
}
