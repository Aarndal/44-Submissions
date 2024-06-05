

public class IdleState : State
{
    private NavMeshMovement _autonomousMover;

    public IdleState(Entity entity, NavMeshMovement autonomousMover) : base(entity)
    {
        _entity = entity;
        _autonomousMover = autonomousMover;
    }
    
    public override void OnEnter()
    {
        _autonomousMover.NavMeshAgent.isStopped = true;
    }
}
