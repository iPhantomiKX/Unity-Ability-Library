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
        PlayerStateMachine.Instance.m_InputReader.TargetEvent += OnCancel;
        PlayerStateMachine.Instance.m_InputReader.AttackEvent += OnAttack;
        PlayerStateMachine.Instance.m_Animator.CrossFadeInFixedTime(m_TargetingBlendTreeHash, PlayerStateMachine.Instance.m_CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if(PlayerStateMachine.Instance.m_InputReader.IsAttacking)
        {
            PlayerStateMachine.Instance.m_NextStateName = "Attacking";
            PlayerStateMachine.Instance.SwitchState(PlayerStateMachine.Instance.GetPlayerStateFromName(PlayerStateMachine.Instance.m_NextStateName));
            return;
        }
        if(PlayerStateMachine.Instance.m_Targeter.m_CurrentTarget == null)
        {
            PlayerStateMachine.Instance.m_IsFocusingEnemy = false;
            PlayerStateMachine.Instance.m_NextStateName = "FreeLooking";
            PlayerStateMachine.Instance.SwitchState(PlayerStateMachine.Instance.GetPlayerStateFromName(PlayerStateMachine.Instance.m_NextStateName));
            return;
        }

        Vector3 movement = CalculateMovement();

        Move(movement * PlayerStateMachine.Instance.m_TargetingMovementSpeed, deltaTime);

        UpdateAnimator(deltaTime);

        FaceTarget();
    }

    private void UpdateAnimator(float deltaTime)
    {
        if(PlayerStateMachine.Instance.m_InputReader.MovementValue.y == 0)
        {
            PlayerStateMachine.Instance.m_Animator.SetFloat(m_TargetingForwardHash, 0, 0.1f, deltaTime);
        }
        else
        {
            float value = PlayerStateMachine.Instance.m_InputReader.MovementValue.y > 0 ? 1f : -1f;
            PlayerStateMachine.Instance.m_Animator.SetFloat(m_TargetingForwardHash, value, 0.1f, deltaTime);
        }

        if (PlayerStateMachine.Instance.m_InputReader.MovementValue.x == 0)
        {
            PlayerStateMachine.Instance.m_Animator.SetFloat(m_TargetingRightHash, 0, 0.1f, deltaTime);
        }
        else
        {
            float value = PlayerStateMachine.Instance.m_InputReader.MovementValue.x > 0 ? 1f : -1f;
            PlayerStateMachine.Instance.m_Animator.SetFloat(m_TargetingRightHash, value, 0.1f, deltaTime);
        }
    }

    public override void Exit()
    {
        PlayerStateMachine.Instance.m_IsFocusingEnemy = false;
        PlayerStateMachine.Instance.m_InputReader.TargetEvent -= OnCancel;
        PlayerStateMachine.Instance.m_InputReader.AttackEvent -= OnAttack;
    }

    private void OnAttack()
    {
        PlayerStateMachine.Instance.m_NextStateName = "Attacking";
        PlayerStateMachine.Instance.SwitchState(PlayerStateMachine.Instance.GetPlayerStateFromName(PlayerStateMachine.Instance.m_NextStateName));
    }

    private void OnCancel()
    {
        PlayerStateMachine.Instance.m_Targeter.Cancel();

        if(PlayerStateMachine.Instance.m_IsFocusingEnemy)
        {
            PlayerStateMachine.Instance.m_IsFocusingEnemy = false;
            PlayerStateMachine.Instance.m_NextStateName = "FreeLooking";
            PlayerStateMachine.Instance.SwitchState(PlayerStateMachine.Instance.GetPlayerStateFromName(PlayerStateMachine.Instance.m_NextStateName));
        }
    }

    private Vector3 CalculateMovement()
    {
        Vector3 movement = new Vector3();

        movement += PlayerStateMachine.Instance.transform.right * PlayerStateMachine.Instance.m_InputReader.MovementValue.x;
        movement += PlayerStateMachine.Instance.transform.forward * PlayerStateMachine.Instance.m_InputReader.MovementValue.y;

        return movement;
    }
}
