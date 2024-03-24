using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    private readonly int m_TargetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");

    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.m_InputReader.TargetEvent += OnCancel;
        stateMachine.m_Animator.Play(m_TargetingBlendTreeHash);
    }

    public override void Tick(float deltaTime)
    {
        if(stateMachine.m_Targeter.m_CurrentTarget == null)
        {
            stateMachine.m_IsFocusingEnemy = false;
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            return;
        }

        Vector3 movement = CalculateMovement();

        Move(movement * stateMachine.m_TargetingMovementSpeed, deltaTime);

        FaceTarget();
    }

    public override void Exit()
    {
        stateMachine.m_InputReader.TargetEvent -= OnCancel;
    }

    private void OnCancel()
    {
        stateMachine.m_Targeter.Cancel();

        if(stateMachine.m_IsFocusingEnemy)
        {
            stateMachine.m_IsFocusingEnemy = false;
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
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
