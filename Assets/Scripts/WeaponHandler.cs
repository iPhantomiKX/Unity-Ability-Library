using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private GameObject m_WeaponLogic;

    public void EnableWeapon() { m_WeaponLogic.SetActive(true); }

    public void DisableWeapon() { m_WeaponLogic.SetActive(false); }
}
