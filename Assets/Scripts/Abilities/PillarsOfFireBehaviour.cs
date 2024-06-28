using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PillarsOfFireBehaviour : MonoBehaviour
{
    [Header("Ability Settings")]
    [SerializeField] List<Rigidbody> m_PillarsOfFire = new List<Rigidbody>();
    [SerializeField] float m_PositionOffset = 3.0f;
    [SerializeField] float m_MaxPos = 5.0f;
    [SerializeField] float m_SpawnTime = 1.0f;
    [SerializeField] float m_Speed = 20.0f;
    [SerializeField] float m_CurrentTime = 0.0f;
    [SerializeField] int m_Count = 0;
    [Space]
    [Header("Internal CD Settings")]
    [SerializeField] float m_Cooldown = 1.0f;
    [SerializeField] float m_CurrentCooldown = 0.0f;

    // Start is called before the first frame update
    void Awake()
    {
        m_PillarsOfFire = GetComponentsInChildren<Rigidbody>().ToList();
        SetPositionOffsetPerPillar();
        m_CurrentCooldown = m_Cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        ActivateAbility();

        if (m_CurrentCooldown <= 0.0f)
            Destroy(gameObject);
        else
            m_CurrentCooldown -= Time.deltaTime;
    }

    void SetPositionOffsetPerPillar()
    {
        for (int i = 1; i < m_PillarsOfFire.Count; ++i)
        {
            m_PillarsOfFire[i].transform.position = new Vector3(m_PillarsOfFire[i].transform.position.x,
                                                                m_PillarsOfFire[i].transform.position.y,
                                                                m_PillarsOfFire[i].transform.position.z + (m_PositionOffset * i));
        }
    }

    void ActivateAbility()
    {
        if (m_Count < m_PillarsOfFire.Count)
        {
            if (m_CurrentTime >= m_SpawnTime)
            {
                m_PillarsOfFire[m_Count].velocity = Vector3.up * m_Speed;
                ++m_Count;
                m_CurrentTime = 0;
            }
            else
                m_CurrentTime += Time.deltaTime;
        }

        foreach (Rigidbody m_PillarOfFire in m_PillarsOfFire)
        {
            if (m_PillarOfFire.transform.position.y >= m_MaxPos)
                m_PillarOfFire.velocity = Vector3.down * m_Speed * 0.5f;
        }
    }
}
