using System;
using UnityEngine;

[Serializable]
public abstract class TargetProvider : MonoBehaviour
{
    public Transform Target { get; protected set; }
    public bool HasTarget { get {  return Target != null && Target.gameObject.activeInHierarchy; } }

    public abstract Transform GetTarget();
}
