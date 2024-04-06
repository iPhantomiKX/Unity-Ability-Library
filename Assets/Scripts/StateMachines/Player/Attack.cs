using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Attack //to be turned into a scriptable object
{
    [field: SerializeField] public string m_AnimationName { get; private set; }
    [field: SerializeField] public float m_TransitionDuration { get; private set; }
    [field: SerializeField] public int m_ComboStateIndex { get; private set; } = -1;
    [field: SerializeField] public float m_ComboAttackTime { get; private set; }
    [field: SerializeField] public float m_ForceTime { get; private set; }
    [field: SerializeField] public float m_Force { get; private set; }
    [field: SerializeField] public int m_Damage { get; private set; }
}

//Create an AttackComboSO
//- Create a list of Attacks
//- Pass them into the PlayerAttackingState
