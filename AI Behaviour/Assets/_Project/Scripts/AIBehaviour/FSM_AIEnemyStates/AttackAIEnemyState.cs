using System.Threading.Tasks;

public class AttackAIEnemyState : AIEnemyState
{
    float _prevMinDistance;

    public AttackAIEnemyState(StateMachine fsm, AIEnemy entity, PlayerTargetProvider targetProvider) : base(fsm, entity, targetProvider) { }

    public async override Task OnEnter()
    {
        _prevMinDistance = AIEnemy.AutonomousMover.MinDistanceToTarget;
        AIEnemy.AutonomousMover.NavMeshAgent.autoBraking = true;
        await Task.Yield();
        AIEnemy.AutonomousMover.NavMeshAgent.stoppingDistance = AIEnemy.AutonomousMover.NavMeshAgent.radius + 1.25f; //To-DO: Change this to a variable
    }

    public override void OnFixedUpdate()
    {
        // Treshhold für Übergänge einfügen
        // Velocity über Transform.position bekommen
        // ExitTime & Transition Duration für Übergang von Attack zu Run einfügen

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
