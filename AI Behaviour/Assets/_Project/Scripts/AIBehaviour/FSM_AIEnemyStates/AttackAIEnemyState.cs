

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

        AIEnemy.AutonomousMover.NavMeshAgent.isStopped = true;
        AIEnemy.AutonomousMover.NavMeshAgent.ResetPath();

        AIEnemy.AutonomousMover.MinDistanceToTarget = 0.1f;
    }

    public override void OnFixedUpdate()
    {
        AIEnemy.AutonomousMover.MoveTo(TargetProvider);
    }

    public override void OnUpdate()
    {
        AIEnemy.Animator.Play("Base Layer.Attack01");
    }

    public override void OnExit()
    {
        AIEnemy.AutonomousMover.MinDistanceToTarget = _prevMinDistance;
    }
}
