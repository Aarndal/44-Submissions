using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(NavMeshMovement))]
public class AIEnemy : Enemy
{
    protected NavMeshMovement _autonomousMover;

    public NavMeshMovement AutonomousMover => _autonomousMover;

    protected override void Awake()
    {
        base.Awake();
        _autonomousMover = AutonomousMover != null ? AutonomousMover : GetComponent<NavMeshMovement>();
    }
}
