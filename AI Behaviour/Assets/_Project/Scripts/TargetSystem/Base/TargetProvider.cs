using System;
using UnityEngine;

[Serializable]
public abstract class TargetProvider : MonoBehaviour
{
    public Transform Target { get; protected set; }
    public bool HasTarget { get { return Target != null && Target.gameObject.activeInHierarchy; } }
    public float SqrDistanceToTarget
    {
        get
        {
            if (HasTarget)
                return (Target.position - transform.parent.transform.position).sqrMagnitude;
            return float.MaxValue; // Maybe change this to null or 0.0f?
        }
    }

    public abstract Transform GetTarget();
}
