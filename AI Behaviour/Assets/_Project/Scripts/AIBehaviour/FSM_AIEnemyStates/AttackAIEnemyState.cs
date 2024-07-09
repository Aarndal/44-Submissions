using System.Threading.Tasks;

public class AttackAIEnemyState : AIEnemyState
{
    float _prevMinDistance;

    public AttackAIEnemyState(StateMachine fsm, AIEnemy entity, TargetProvider targetProvider) : base(fsm, entity, targetProvider) { }

    public async override Task OnEnter()
    {
        _prevMinDistance = AIEnemy.AutonomousMover.MinDistanceToTarget;
        AIEnemy.AutonomousMover.MinDistanceToTarget = AIEnemy.AutonomousMover.NavMeshAgent.radius + 1.25f; //To-DO: Change this to a variable
        AIEnemy.AutonomousMover.NavMeshAgent.autoBraking = true;

        await Task.Yield();
    }

    public override void OnFixedUpdate()
    {
        if (TargetProvider.SqrDistanceToTarget <= AIEnemy.AutonomousMover.MinDistanceToTarget * AIEnemy.AutonomousMover.MinDistanceToTarget)
        {
            AIEnemy.AutonomousMover.NavMeshAgent.isStopped = true;
            AIEnemy.Animator.Play("Base Layer.Attack02");
            AIEnemy.AutonomousMover.NavMeshAgent.ResetPath();
        }
        else
        {
            AIEnemy.AutonomousMover.NavMeshAgent.isStopped = false;
            AIEnemy.Animator.Play("Base Layer.Run");
            AIEnemy.AutonomousMover.MoveTo(TargetProvider);
        }
    }

    public async override Task OnExit()
    {
        AIEnemy.AutonomousMover.MinDistanceToTarget = _prevMinDistance;
        await Task.Yield();
    }
}
