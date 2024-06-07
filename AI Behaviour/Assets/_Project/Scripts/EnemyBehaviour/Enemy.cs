

public class Enemy : Entity
{
    protected NavMeshMovement _autonomousMover;

    public NavMeshMovement AutonomousMover => _autonomousMover;

    protected void Awake()
    {
        _autonomousMover = AutonomousMover != null ? AutonomousMover : GetComponent<NavMeshMovement>();
    }
}
