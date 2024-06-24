

public class AttackAIEnemyState : AIEnemyState
{
    float _prevMinDistance;

    public AttackAIEnemyState(AIEnemy entity, TargetProvider targetProvider) : base(entity, targetProvider)
    {
    }

    public override void OnEnter()
    {
        _prevMinDistance = AIEnemy.AutonomousMover.MinDistanceToTarget;
        AIEnemy.AutonomousMover.MinDistanceToTarget = 0.0f;
        AIEnemy.Animator.Play("Base Layer.Attack01");
        //AIEnemy.AutonomousMover.MoveTo(TargetProvider);
    }

    public override void OnExit()
    {
        AIEnemy.AutonomousMover.MinDistanceToTarget = _prevMinDistance;
    }
}
