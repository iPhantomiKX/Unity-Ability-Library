using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCastAbilityState : PlayerBaseState
{
    public float m_Runtime = 0.0f;  //Adjustable
    public Vector3 m_Offset = Vector3.zero;
    public GameObject m_AbilityGO = null;   //ideally cal an AbilitySO
    GameObject m_AbilityInstanceGO = null;

    public override void Enter()
    {
        stateMachine.m_Animator.CrossFadeInFixedTime(stateMachine.m_Attacks[2].m_AnimationName, stateMachine.m_Attacks[2].m_TransitionDuration);    //Animation should be using abilitySO's data
        m_AbilityInstanceGO = Instantiate(m_AbilityGO);
        m_AbilityInstanceGO.transform.position = stateMachine.transform.position + (stateMachine.transform.right * m_Offset.x) 
                                                                                 + (stateMachine.transform.forward * m_Offset.z) 
                                                                                 + (stateMachine.transform.up * m_Offset.y);
        m_AbilityInstanceGO.transform.rotation = stateMachine.transform.rotation;
    }

    public override void Tick(float deltaTime)
    {
        //Call Ability Update Here
        //stateMachine.PlayerCurrentAbility.Update(deltaTime);
        if (m_Runtime >= stateMachine.m_Attacks[2].m_ComboAttackTime)
            stateMachine.SwitchState(stateMachine.GetPlayerStateFromName(stateMachine.m_NextStateName));
        else
            m_Runtime += deltaTime;
    }

    public override void Exit()
    {
        //reset the timer
        m_Runtime = 0.0f;
    }
}
