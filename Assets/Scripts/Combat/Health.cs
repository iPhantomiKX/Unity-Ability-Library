using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int m_MaxHealth = 100;   //temporarily int

    private int m_CurrentHealth;

    // Start is called before the first frame update
    private void Start()
    {
        m_CurrentHealth = m_MaxHealth;
    }

    public void DealDamage(int damage)
    {
        m_CurrentHealth = Mathf.Max(m_CurrentHealth - damage, 0);

        if (m_CurrentHealth == 0)
        {
            if(!gameObject.CompareTag("Player"))
            {
                Destroy(gameObject);
            }
            return;
        }

        Debug.LogError(gameObject.name + " Health: " + m_CurrentHealth);
    }
}
