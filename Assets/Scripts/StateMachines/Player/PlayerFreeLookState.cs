using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    public readonly int m_FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
    public readonly int m_FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
    public float m_AnimatorDampTime = 0.1f;

    public override void Enter()
    {
        stateMachine.m_InputReader.AttackEvent += OnAttack;
        stateMachine.m_InputReader.DodgeEvent += OnDodge;
        stateMachine.m_InputReader.CastingAbilityEvent += OnCastAbility;
        stateMachine.m_Animator.CrossFadeInFixedTime(m_FreeLookBlendTreeHash, stateMachine.m_CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = CalculateMovement();

        Move(movement * stateMachine.m_FreeLookMovementSpeed, deltaTime);

        if (stateMachine.m_InputReader.MovementValue == Vector2.zero)
        {
            stateMachine.m_Animator.SetFloat(m_FreeLookSpeedHash, 0, m_AnimatorDampTime, deltaTime);
            return;
        }

        stateMachine.m_Animator.SetFloat(m_FreeLookSpeedHash, 1, m_AnimatorDampTime, deltaTime);
        FacemovementDirection(movement, deltaTime);
    }

    public override void Exit()
    {
        stateMachine.m_InputReader.AttackEvent -= OnAttack;
        stateMachine.m_InputReader.DodgeEvent -= OnDodge;
        stateMachine.m_InputReader.CastingAbilityEvent -= OnCastAbility;
    }

    private void OnAttack()
    {
        stateMachine.m_NextStateName = "Attacking";
    }

    private void OnDodge()
    {
        stateMachine.m_NextStateName = "Dodging";
    }

    private void OnCastAbility()
    {
        stateMachine.m_NextStateName = "CastAbility";
    }

    private Vector3 CalculateMovement()
    {
        Vector3 forward = stateMachine.m_MainCameraTransform.forward;
        Vector3 right = stateMachine.m_MainCameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.m_InputReader.MovementValue.y +
               right * stateMachine.m_InputReader.MovementValue.x;
    }

    private void FacemovementDirection(Vector3 movement, float deltaTime)
    {
        stateMachine.transform.rotation = Quaternion.Lerp(
                                          stateMachine.transform.rotation, 
                                          Quaternion.LookRotation(movement),
                                          deltaTime * stateMachine.m_RotationDamping);
    }
}

