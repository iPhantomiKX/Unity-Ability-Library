using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController m_Controller;
    [SerializeField] private float m_Drag = 0.3f;
    private Vector3 m_Impact;
    private Vector3 m_DampingVelocity;
    private float m_VerticalVelocity;
    public Vector3 m_Movement => Vector3.up * m_VerticalVelocity;

    private void Update()
    {
        DampForce();
        if (m_VerticalVelocity < 0f && m_Controller.isGrounded)
        {
            m_VerticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            m_VerticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
    }

    public void AddForce(Vector3 force)
    {
        m_Impact += force;
    }

    public void DampForce()
    {
        m_Impact = Vector3.SmoothDamp(m_Impact, Vector3.zero, ref m_DampingVelocity, m_Drag);
    }

    public Vector3 GetImpact() { return m_Impact; }
}
