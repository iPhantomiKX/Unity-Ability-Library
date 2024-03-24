using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup m_CineTargetGroup;
    public List<Target> m_Targets = new List<Target>();
    public Target m_CurrentTarget { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Target target) || m_Targets.Contains(target)) return;

        m_Targets.Add(target);
        target.OnDestroyed += RemoveTarget;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out Target target) || !m_Targets.Contains(target)) return;

        RemoveTarget(target);
    }

    public bool SelectTarget()
    {
        if(m_Targets.Count == 0) return false;

        m_CurrentTarget = m_Targets[0];
        m_CineTargetGroup.AddMember(m_CurrentTarget.transform, 1f, 2f);

        return true;
    }

    public void Cancel()
    {
        if(m_CurrentTarget == null) return;

        m_CineTargetGroup.RemoveMember(m_CurrentTarget.transform);
        m_CurrentTarget = null;
    }

    private void RemoveTarget(Target target)
    {
        if(m_CurrentTarget == target)
        {
            m_CineTargetGroup.RemoveMember(m_CurrentTarget.transform);
            m_CurrentTarget = null;
        }

        target.OnDestroyed -= RemoveTarget;
        m_Targets.Remove(target);
    }
}
