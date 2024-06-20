

public class AIEnemyState : State
{
    public AIEnemy AIEnemy { get; protected set; }
    public TargetProvider TargetProvider { get; protected set; }

    public AIEnemyState(AIEnemy entity, TargetProvider targetProvider) : base(entity)
    {
        AIEnemy = entity;
        TargetProvider = targetProvider;
    }
}
