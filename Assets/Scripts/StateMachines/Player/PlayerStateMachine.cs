using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField] public InputReader m_InputReader {  get; private set; }
    [field: SerializeField] public CharacterController m_Controller {  get; private set; }
    [field: SerializeField] public Animator m_Animator {  get; private set; }
    [field: SerializeField] public Targeter m_Targeter {  get; private set; }
    [field: SerializeField] public ForceReceiver m_ForceReceiver {  get; private set; }
    [field: SerializeField] public WeaponDamage m_Weapon { get; private set; }
    [field: SerializeField] public float m_FreeLookMovementSpeed {  get; private set; }
    [field: SerializeField] public float m_TargetingMovementSpeed {  get; private set; }
    [field: SerializeField] public float m_RotationDamping { get; private set; }
    [field: SerializeField] public bool m_IsFocusingEnemy;
    [field: SerializeField] public Attack[] m_Attacks { get; private set; }
    [field: SerializeField] public float m_CurrentAttackTimer = 0f;
    [field: SerializeField] public float m_CurrentAnimNormalizedTime = 0f;
    [field: SerializeField] public float m_MaxAttackTimer = 1f;
    [field: SerializeField] public float m_CrossFadeDuration = 0.1f;

    public Transform m_MainCameraTransform {  get; private set; }

    private void Start()
    {
        m_MainCameraTransform = Camera.main.transform;
        SwitchState(new PlayerFreeLookState(this));
    }
}

