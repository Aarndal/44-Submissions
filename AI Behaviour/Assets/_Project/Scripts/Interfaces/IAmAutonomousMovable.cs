using UnityEngine;

public interface IAmAutonomousMovable
{
    public bool ReachedTarget { get; }
    public float DistanceToTarget { get; }
    public float MaxDistanceToTarget { get; set; }
    public Vector3 CurrentPosition { get; }

    void MoveTo(TargetProvider targetProvider);
}
