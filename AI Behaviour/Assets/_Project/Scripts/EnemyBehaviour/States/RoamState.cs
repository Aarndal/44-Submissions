

public class RoamState : State
{
    private NavMeshMovement _autonomousMover;

    public RoamState(Entity entity, NavMeshMovement autonomousMover) : base(entity)
    {
        _entity = entity;
        _autonomousMover = autonomousMover;
    }

    public override void OnEnter()
    {
        _autonomousMover.NavMeshAgent.isStopped = false;
    }
}
