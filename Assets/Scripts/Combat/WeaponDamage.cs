using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] private Collider m_OwnCollider;
    private List<Collider> m_AlreadyCollidedWith = new List<Collider>();
    private int m_WeaponDamage;

    private void OnEnable()
    {
        m_AlreadyCollidedWith.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == m_OwnCollider) return;

        if(m_AlreadyCollidedWith.Contains(other)) return;

        m_AlreadyCollidedWith.Add(other);

        if(other.TryGetComponent(out Health health))
        {
            health.DealDamage(m_WeaponDamage);
        }
    }

    public void SetDamage(int damage)
    {
        m_WeaponDamage = damage;
    }
}
