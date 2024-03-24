using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField] public InputReader m_InputReader {  get; private set; }
    [field: SerializeField] public CharacterController m_Controller {  get; private set; }
    [field: SerializeField] public Animator m_Animator {  get; private set; }
    [field: SerializeField] public Targeter m_Targeter {  get; private set; }
    [field: SerializeField] public ForceReceiver m_ForceReceiver {  get; private set; }
    [field: SerializeField] public float m_FreeLookMovementSpeed {  get; private set; }
    [field: SerializeField] public float m_TargetingMovementSpeed {  get; private set; }
    [field: SerializeField] public float m_RotationDamping {  get; private set; }
    [field: SerializeField] public bool m_IsFocusingEnemy;

    public Transform m_MainCameraTransform {  get; private set; }

    private void Start()
    {
        m_MainCameraTransform = Camera.main.transform;
        SwitchState(new PlayerFreeLookState(this));
    }
}

