
//[CreateAssetMenu(fileName = "ChaseState", menuName = "AI/States/ChaseState")]
public sealed class ChaseAIEnemyState : AIEnemyState
{
    public ChaseAIEnemyState(AIEnemy entity, TargetProvider targetProvider) : base(entity, targetProvider)
    {
    }

    public override void OnEnter()
    {
        AIEnemy.AutonomousMover.NavMeshAgent.enabled = true;
        AIEnemy.AutonomousMover.NavMeshAgent.isStopped = false;
    }

    public override void OnUpdate()
    {
        AIEnemy.AutonomousMover.MoveTo(TargetProvider);
    }
}
