using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityActivator : MonoBehaviour
{
    [SerializeField] GameObject m_Ability = null;
    GameObject m_Internal = null;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            SpawnAbility();
        }
    }

    void SpawnAbility()
    {
        m_Internal = Instantiate(m_Ability);
        m_Internal.transform.rotation = transform.rotation;
    }
}
