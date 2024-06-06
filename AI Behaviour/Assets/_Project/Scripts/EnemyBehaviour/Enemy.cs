

public class Enemy : Entity
{
    protected NavMeshMovement _autonomousMover;

    public NavMeshMovement AutonomousMover => _autonomousMover;

    private void Awake()
    {
        _autonomousMover = AutonomousMover != null ? AutonomousMover : GetComponentInChildren<NavMeshMovement>();
    }
}
