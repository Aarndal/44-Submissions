using UnityEngine;

public abstract class TargetProvider : MonoBehaviour
{
    public Transform Target { get; set; }
    public bool HasTarget { get {  return Target != null && Target.gameObject.activeInHierarchy; } }

    public abstract Transform GetTarget();
}
