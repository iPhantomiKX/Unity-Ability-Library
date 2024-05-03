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
        ((PlayerStateMachine)m_StateMachine).m_InputReader.TargetEvent += OnTarget;
        ((PlayerStateMachine)m_StateMachine).m_InputReader.AttackEvent += OnAttack;
        ((PlayerStateMachine)m_StateMachine).m_Animator.CrossFadeInFixedTime(m_FreeLookBlendTreeHash, ((PlayerStateMachine)m_StateMachine).m_CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = CalculateMovement();

        Move(movement * ((PlayerStateMachine)m_StateMachine).m_FreeLookMovementSpeed, deltaTime);

        if (((PlayerStateMachine)m_StateMachine).m_InputReader.MovementValue == Vector2.zero)
        {
            ((PlayerStateMachine)m_StateMachine).m_Animator.SetFloat(m_FreeLookSpeedHash, 0, m_AnimatorDampTime, deltaTime);
            return;
        }

        ((PlayerStateMachine)m_StateMachine).m_Animator.SetFloat(m_FreeLookSpeedHash, 1, m_AnimatorDampTime, deltaTime);
        FacemovementDirection(movement, deltaTime);
    }

    public override void Exit()
    {
        ((PlayerStateMachine)m_StateMachine).m_InputReader.TargetEvent -= OnTarget;
        ((PlayerStateMachine)m_StateMachine).m_InputReader.AttackEvent -= OnAttack;
    }

    private void OnAttack()
    {
        ((PlayerStateMachine)m_StateMachine).m_NextStateName = "Attacking";
        ((PlayerStateMachine)m_StateMachine).SwitchState(((PlayerStateMachine)m_StateMachine).GetPlayerStateFromName(((PlayerStateMachine)m_StateMachine).m_NextStateName), ((PlayerStateMachine)m_StateMachine));
    }

    private void OnTarget()
    {
        if (!((PlayerStateMachine)m_StateMachine).m_Targeter.SelectTarget()) return;
        
        if(!((PlayerStateMachine)m_StateMachine).m_IsFocusingEnemy)
        {
            ((PlayerStateMachine)m_StateMachine).m_IsFocusingEnemy = true;
            ((PlayerStateMachine)m_StateMachine).m_NextStateName = "Targeting";
            ((PlayerStateMachine)m_StateMachine).SwitchState(((PlayerStateMachine)m_StateMachine).GetPlayerStateFromName(((PlayerStateMachine)m_StateMachine).m_NextStateName), ((PlayerStateMachine)m_StateMachine));
        }
    }

    private Vector3 CalculateMovement()
    {
        Vector3 forward = ((PlayerStateMachine)m_StateMachine).m_MainCameraTransform.forward;
        Vector3 right = ((PlayerStateMachine)m_StateMachine).m_MainCameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        return forward * ((PlayerStateMachine)m_StateMachine).m_InputReader.MovementValue.y +
               right * ((PlayerStateMachine)m_StateMachine).m_InputReader.MovementValue.x;
    }

    private void FacemovementDirection(Vector3 movement, float deltaTime)
    {
        ((PlayerStateMachine)m_StateMachine).transform.rotation = Quaternion.Lerp(
                                          ((PlayerStateMachine)m_StateMachine).transform.rotation, 
                                          Quaternion.LookRotation(movement),
                                          deltaTime * ((PlayerStateMachine)m_StateMachine).m_RotationDamping);
    }
}

