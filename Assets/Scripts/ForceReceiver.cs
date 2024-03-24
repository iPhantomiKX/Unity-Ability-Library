using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController m_Controller;
    private float m_VerticalVelocity;
    public Vector3 m_Movement => Vector3.up * m_VerticalVelocity;

    private void Update()
    {
        if (m_VerticalVelocity < 0f && m_Controller.isGrounded)
        {
            m_VerticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            m_VerticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
    }
}
