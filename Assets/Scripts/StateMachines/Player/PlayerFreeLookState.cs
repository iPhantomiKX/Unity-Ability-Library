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
        PlayerStateMachine.Instance.m_InputReader.TargetEvent += OnTarget;
        PlayerStateMachine.Instance.m_InputReader.AttackEvent += OnAttack;
        PlayerStateMachine.Instance.m_Animator.CrossFadeInFixedTime(m_FreeLookBlendTreeHash, PlayerStateMachine.Instance.m_CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = CalculateMovement();

        Move(movement * PlayerStateMachine.Instance.m_FreeLookMovementSpeed, deltaTime);

        if (PlayerStateMachine.Instance.m_InputReader.MovementValue == Vector2.zero)
        {
            PlayerStateMachine.Instance.m_Animator.SetFloat(m_FreeLookSpeedHash, 0, m_AnimatorDampTime, deltaTime);
            return;
        }

        PlayerStateMachine.Instance.m_Animator.SetFloat(m_FreeLookSpeedHash, 1, m_AnimatorDampTime, deltaTime);
        FacemovementDirection(movement, deltaTime);
    }

    public override void Exit()
    {
        PlayerStateMachine.Instance.m_InputReader.TargetEvent -= OnTarget;
        PlayerStateMachine.Instance.m_InputReader.AttackEvent -= OnAttack;
    }

    private void OnAttack()
    {
        PlayerStateMachine.Instance.m_NextStateName = "Attacking";
        PlayerStateMachine.Instance.SwitchState(PlayerStateMachine.Instance.GetPlayerStateFromName(PlayerStateMachine.Instance.m_NextStateName));
    }

    private void OnTarget()
    {
        if (!PlayerStateMachine.Instance.m_Targeter.SelectTarget()) return;
        
        if(!PlayerStateMachine.Instance.m_IsFocusingEnemy)
        {
            PlayerStateMachine.Instance.m_IsFocusingEnemy = true;
            PlayerStateMachine.Instance.m_NextStateName = "Targeting";
            PlayerStateMachine.Instance.SwitchState(PlayerStateMachine.Instance.GetPlayerStateFromName(PlayerStateMachine.Instance.m_NextStateName));
        }
    }

    private Vector3 CalculateMovement()
    {
        Vector3 forward = PlayerStateMachine.Instance.m_MainCameraTransform.forward;
        Vector3 right = PlayerStateMachine.Instance.m_MainCameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        return forward * PlayerStateMachine.Instance.m_InputReader.MovementValue.y +
               right * PlayerStateMachine.Instance.m_InputReader.MovementValue.x;
    }

    private void FacemovementDirection(Vector3 movement, float deltaTime)
    {
        PlayerStateMachine.Instance.transform.rotation = Quaternion.Lerp(
                                          PlayerStateMachine.Instance.transform.rotation, 
                                          Quaternion.LookRotation(movement),
                                          deltaTime * PlayerStateMachine.Instance.m_RotationDamping);
    }
}

