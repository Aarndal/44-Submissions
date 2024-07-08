using System.Threading.Tasks;

//[CreateAssetMenu(fileName = "ChaseState", menuName = "AI/States/ChaseState")]
public sealed class ChaseAIEnemyState : AIEnemyState
{
    private float _prevStoppingDistance;

    public ChaseAIEnemyState(StateMachine fsm, AIEnemy entity, TargetProvider targetProvider) : base(fsm, entity, targetProvider)
    {
    }

    public async override Task OnEnter()
    {
        AIEnemy.AutonomousMover.NavMeshAgent.autoBraking = false;

        AIEnemy.AutonomousMover.NavMeshAgent.isStopped = false;
        AIEnemy.AutonomousMover.NavMeshAgent.ResetPath();

        AIEnemy.AutonomousMover.NavMeshAgent.speed = 5.0f;
        await Task.Yield();

        FSM.History.Pop();

        if (FSM.History.Peek() is IdleAIEnemyState or RoamAIEnemyState)
            AIEnemy.Animator.Play("Base Layer.Howl");

        FSM.History.Push(this);
    }

    public override void OnFixedUpdate()
    {
        if (AIEnemy.Animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Run"))
            AIEnemy.AutonomousMover.MoveTo(TargetProvider);
    }

    public override void OnUpdate()
    {
        if (AIEnemy.Animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Howl"))
            AIEnemy.Animator.SetBool("HasHowled", true);
    }

    public async override Task OnExit()
    {
        AIEnemy.AutonomousMover.NavMeshAgent.autoBraking = true;
        await Task.Yield();
    }
}
