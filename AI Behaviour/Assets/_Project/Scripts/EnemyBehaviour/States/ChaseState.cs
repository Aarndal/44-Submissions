

public class ChaseState : State
{
    private NavMeshMovement _autonomousMover;

    public ChaseState(Entity entity, NavMeshMovement autonomousMover) : base(entity)
    {
        _entity = entity;
        _autonomousMover = autonomousMover;
    }

    public override void OnEnter()
    {
        _autonomousMover.NavMeshAgent.isStopped = false;
    }
}
