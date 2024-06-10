

public class Enemy : Entity
{
    protected NavMeshMovement _autonomousMover;

    public NavMeshMovement AutonomousMover => _autonomousMover;

    protected override void Awake()
    {
        base.Awake();
        _autonomousMover = AutonomousMover != null ? AutonomousMover : GetComponentInChildren<NavMeshMovement>();
    }
}
