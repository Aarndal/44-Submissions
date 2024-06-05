

public class HuntingState : State
{
    public HuntingState(Entity entity) : base(entity)
    {
    }

    public override void OnEnter()
    {
        _entity.NavMeshAgent.isStopped = false;
    }
}
