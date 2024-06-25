

public class AttackAIEnemyState : AIEnemyState
{
    float _prevMinDistance;
    bool _hasAttacked;

    public AttackAIEnemyState(AIEnemy entity, TargetProvider targetProvider) : base(entity, targetProvider)
    {
    }

    public override void OnEnter()
    {
        _prevMinDistance = AIEnemy.AutonomousMover.MinDistanceToTarget;

        AIEnemy.AutonomousMover.NavMeshAgent.isStopped = false;
        AIEnemy.AutonomousMover.MinDistanceToTarget = 2.0f;
    }

    public override void OnUpdate()
    {
        AIEnemy.Animator.Play("Base Layer.Attack01");
        AIEnemy.AutonomousMover.MoveTo(TargetProvider);
    }

    public override void OnExit()
    {
        AIEnemy.AutonomousMover.MinDistanceToTarget = _prevMinDistance;
    }
}
