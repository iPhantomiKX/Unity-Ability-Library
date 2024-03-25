using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup m_CineTargetGroup;
    private Camera m_MainCamera;
    public List<Target> m_Targets = new List<Target>();
    public Target m_CurrentTarget { get; private set; }

    private void Start()
    {
        m_MainCamera = Camera.main;
    }

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

        Target closestTarget = null;
        float closestTargetDistance = Mathf.Infinity;

        foreach(Target target in m_Targets)
        {
            Vector2 viewPos = m_MainCamera.WorldToViewportPoint(target.transform.position);

            if (viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1) continue;

            Vector2 toCenter = viewPos - new Vector2(0.5f, 0.5f);
            if(toCenter.sqrMagnitude < closestTargetDistance)
            {
                closestTarget = target;
                closestTargetDistance = toCenter.sqrMagnitude;
            }
        }

        if(closestTarget == null) return false;

        m_CurrentTarget = closestTarget;
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
