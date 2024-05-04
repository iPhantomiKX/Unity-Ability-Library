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
        stateMachine.m_InputReader.TargetEvent += OnCancel;
        stateMachine.m_InputReader.AttackEvent += OnAttack;
        stateMachine.m_Animator.CrossFadeInFixedTime(m_TargetingBlendTreeHash, stateMachine.m_CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if(stateMachine.m_InputReader.IsAttacking)
        {
            stateMachine.m_NextStateName = "Attacking";
            stateMachine.SwitchState(stateMachine.GetPlayerStateFromName(stateMachine.m_NextStateName));
            return;
        }
        if(stateMachine.m_Targeter.m_CurrentTarget == null)
        {
            stateMachine.m_IsFocusingEnemy = false;
            stateMachine.m_NextStateName = "FreeLooking";
            stateMachine.SwitchState(stateMachine.GetPlayerStateFromName(stateMachine.m_NextStateName));
            return;
        }

        Vector3 movement = CalculateMovement();

        Move(movement * stateMachine.m_TargetingMovementSpeed, deltaTime);

        UpdateAnimator(deltaTime);

        FaceTarget();
    }

    private void UpdateAnimator(float deltaTime)
    {
        if(stateMachine.m_InputReader.MovementValue.y == 0)
        {
            stateMachine.m_Animator.SetFloat(m_TargetingForwardHash, 0, 0.1f, deltaTime);
        }
        else
        {
            float value = stateMachine.m_InputReader.MovementValue.y > 0 ? 1f : -1f;
            stateMachine.m_Animator.SetFloat(m_TargetingForwardHash, value, 0.1f, deltaTime);
        }

        if (stateMachine.m_InputReader.MovementValue.x == 0)
        {
            stateMachine.m_Animator.SetFloat(m_TargetingRightHash, 0, 0.1f, deltaTime);
        }
        else
        {
            float value = stateMachine.m_InputReader.MovementValue.x > 0 ? 1f : -1f;
            stateMachine.m_Animator.SetFloat(m_TargetingRightHash, value, 0.1f, deltaTime);
        }
    }

    public override void Exit()
    {
        stateMachine.m_IsFocusingEnemy = false;
        stateMachine.m_InputReader.TargetEvent -= OnCancel;
        stateMachine.m_InputReader.AttackEvent -= OnAttack;
    }

    private void OnAttack()
    {
        stateMachine.m_NextStateName = "Attacking";
        stateMachine.SwitchState(stateMachine.GetPlayerStateFromName(stateMachine.m_NextStateName));
    }

    private void OnCancel()
    {
        stateMachine.m_Targeter.Cancel();

        if(stateMachine.m_IsFocusingEnemy)
        {
            stateMachine.m_IsFocusingEnemy = false;
            stateMachine.m_NextStateName = "FreeLooking";
            stateMachine.SwitchState(stateMachine.GetPlayerStateFromName(stateMachine.m_NextStateName));
        }
    }

    private Vector3 CalculateMovement()
    {
        Vector3 movement = new Vector3();

        movement += stateMachine.transform.right * stateMachine.m_InputReader.MovementValue.x;
        movement += stateMachine.transform.forward * stateMachine.m_InputReader.MovementValue.y;

        return movement;
    }
}
