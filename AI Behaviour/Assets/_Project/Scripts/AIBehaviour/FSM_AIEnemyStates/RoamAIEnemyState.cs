using System.Threading.Tasks;

//[CreateAssetMenu(fileName = "RoamState", menuName = "AI/States/RoamState")]
public sealed class RoamAIEnemyState : AIEnemyState
{
    private float _prevStoppingDistance;

    public float Radius { get; private set; }

    public RoamAIEnemyState(StateMachine fsm, AIEnemy entity, RandomWayPointTargetProvider targetProvider) : base(fsm, entity, targetProvider) { }

    public async override Task OnEnter()
    {
        AIEnemy.AutonomousMover.NavMeshAgent.isStopped = false;
        AIEnemy.AutonomousMover.NavMeshAgent.autoRepath = true;
        AIEnemy.AutonomousMover.NavMeshAgent.autoBraking = true;

        _prevStoppingDistance = AIEnemy.AutonomousMover.MinDistanceToTarget;
        AIEnemy.AutonomousMover.MinDistanceToTarget = 0.5f;

        AIEnemy.AutonomousMover.NavMeshAgent.ResetPath();

        AIEnemy.AutonomousMover.NavMeshAgent.speed = 2.0f;

        TargetProvider.GetTarget();

        await Task.Yield();

        AIEnemy.AutonomousMover.MoveTo(TargetProvider);
    }

    public async override void OnFixedUpdate()
    {
        if (AIEnemy.AutonomousMover.ReachedTarget || AIEnemy.AutonomousMover.NavMeshAgent.isPathStale || !AIEnemy.AutonomousMover.NavMeshAgent.hasPath)
        {
            TargetProvider.GetTarget();
            await Task.Yield();
            AIEnemy.AutonomousMover.MoveTo(TargetProvider);
        }
    }

    public override void OnUpdate()
    {
        if (AIEnemy.AutonomousMover.NavMeshAgent.velocity.sqrMagnitude <= 0.01f)
            AIEnemy.Animator.Play("Base Layer.Idle");
        else
            AIEnemy.Animator.Play("Base Layer.Walk");
    }

    public async override Task OnExit()
    {
        AIEnemy.AutonomousMover.MinDistanceToTarget = _prevStoppingDistance;
        await Task.Yield();
    }
}
