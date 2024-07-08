using System.Threading.Tasks;

public class AttackAIEnemyState : AIEnemyState
{
    float _prevMinDistance;
    bool _hasAttacked;

    public AttackAIEnemyState(StateMachine fsm, AIEnemy entity, TargetProvider targetProvider) : base(fsm, entity, targetProvider)
    {
    }

    public async override Task OnEnter()
    {
        _prevMinDistance = AIEnemy.AutonomousMover.MinDistanceToTarget;

        AIEnemy.AutonomousMover.NavMeshAgent.isStopped = true;
        AIEnemy.AutonomousMover.NavMeshAgent.ResetPath();

        AIEnemy.AutonomousMover.MinDistanceToTarget = 0.1f;
        
        await Task.Yield();
    }

    public override void OnFixedUpdate()
    {
        AIEnemy.AutonomousMover.MoveTo(TargetProvider);
    }

    public override void OnUpdate()
    {
        AIEnemy.Animator.Play("Base Layer.Attack01");
    }

    public async override Task OnExit()
    {
        AIEnemy.AutonomousMover.MinDistanceToTarget = _prevMinDistance;
        await Task.Yield();
    }
}
