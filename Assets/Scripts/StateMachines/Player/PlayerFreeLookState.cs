using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    private readonly int m_FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
    private readonly int m_FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
    private const float m_AnimatorDampTime = 0.1f;

    public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.m_InputReader.TargetEvent += OnTarget;
        stateMachine.m_Animator.Play(m_FreeLookBlendTreeHash);
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.m_InputReader.IsAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
            return;
        }

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
        stateMachine.m_InputReader.TargetEvent -= OnTarget;
    }

    private void OnTarget()
    {
        if (!stateMachine.m_Targeter.SelectTarget()) return;
        
        if(!stateMachine.m_IsFocusingEnemy)
        {
            stateMachine.m_IsFocusingEnemy = true;
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
        }
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

