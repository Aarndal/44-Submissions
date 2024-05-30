using UnityEngine;

public interface IAmAutonomousMovable
{
    bool IsTargetReached { get; }
    public float DistanceToTarget { get; }
    public float MaxDistanceToTarget { get; set; }
    public Vector3 CurrentPosition { get; }

    void Move(TargetProvider targetProvider);
}
