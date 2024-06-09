using System;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Collections;
using UnityEditorInternal;
using UnityEngine;
using ReadOnly = Unity.Collections.ReadOnlyAttribute;

public class PlayerStateMachine : StateMachine  //A Player behavioural controller
{
    //keep a list of states, then create a function here to update statemachine to go to those states
    [Header("State Machine")]
    [field: SerializeField] PlayerBaseState m_InitialState = null;
    [field: SerializeField] List<PlayerBaseState> m_AllStates = default;

    Dictionary<string, PlayerBaseState> m_PlayerStates = new Dictionary<string, PlayerBaseState>();
    private PlayerBaseState m_CurrPlayerState = null;
    public PlayerBaseState CurrState => m_CurrPlayerState;
    private PlayerBaseState m_NextPlayerState = null;
    public PlayerBaseState NextState => m_NextPlayerState;
    public string m_NextStateName;  //could make it such that PlayerStateMachine actually controls the transition

    [field: SerializeField] public InputReader m_InputReader { get; private set; }
    [field: SerializeField] public CharacterController m_Controller { get; private set; }
    [field: SerializeField] public Animator m_Animator { get; private set; }
    [field: SerializeField] public Targeter m_Targeter { get; private set; }
    [field: SerializeField] public ForceReceiver m_ForceReceiver { get; private set; }
    [field: SerializeField] public WeaponDamage m_Weapon { get; private set; }
    [field: SerializeField] public float m_FreeLookMovementSpeed { get; private set; }
    [field: SerializeField] public float m_TargetingMovementSpeed { get; private set; }
    [field: SerializeField] public float m_RotationDamping { get; private set; }
    [field: SerializeField] public bool m_IsFocusingEnemy;
    [field: SerializeField] public Attack[] m_Attacks { get; private set; }
    [field: SerializeField] public float m_CurrentAttackTimer = 0f;
    [field: SerializeField] public float m_CurrentAnimNormalizedTime = 0f;
    [field: SerializeField] public float m_MaxAttackTimer = 1f;
    [field: SerializeField] public float m_CrossFadeDuration = 0.1f;

    public Transform m_MainCameraTransform { get; private set; }
    private Dictionary<PlayerBaseState, int> m_StateCount = new Dictionary<PlayerBaseState, int>();

#region PlayerAbilities_Settings
    //Reference to Ability Here:
    //public Queue<Ability> PlayerAbilities;
    //public Ability PlayerCurrentAbility;
#endregion

    public static PlayerStateMachine Instance
    {
        get
        {
            if (_Instance == null)
                _Instance = new PlayerStateMachine();
            return _Instance;
        }
    }
    private static PlayerStateMachine _Instance; 

    private void Start()
    {
        _Instance = this;
        m_MainCameraTransform = Camera.main.transform;
        foreach (PlayerBaseState playerState in m_AllStates)
        {
            AddStateToPlayerStateDict(playerState);
        }
        m_CurrPlayerState = m_InitialState;
        SwitchState(m_CurrPlayerState);
    }

    private void AddStateToPlayerStateDict(PlayerBaseState playerState)
    {
        if(m_StateCount.ContainsKey(playerState))
        {
            m_StateCount[playerState]++;
            return;
        }
        m_StateCount.Add(playerState, 1);
        m_PlayerStates.Add(playerState.m_StateName, playerState);
    }

    //do update here somehow
    public override void Update()
    {
        base.Update();
        UpdateState();
    }

    private void UpdateState()
    {
        if (m_NextStateName == "") return;

        m_NextPlayerState = GetPlayerStateFromName(m_NextStateName);

        if (m_NextPlayerState != null)
        {
            m_CurrPlayerState = m_NextPlayerState;
            SwitchState(m_CurrPlayerState);
            m_NextPlayerState = null;
            m_NextStateName = "";
        }
    }

    public PlayerBaseState GetPlayerStateFromName(string name)
    {
        if (name == string.Empty || !m_PlayerStates.ContainsKey(name))
            return m_InitialState;

        return m_PlayerStates[name];
    }
}