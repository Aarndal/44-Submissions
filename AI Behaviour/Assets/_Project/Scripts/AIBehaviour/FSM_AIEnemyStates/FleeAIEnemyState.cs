using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class FleeAIEnemyState : AIEnemyState
{
    private float _fleeDistance = 100f;
    
    public float FleeDistance { get => _fleeDistance; set => _fleeDistance = value; }

    public FleeAIEnemyState(StateMachine fsm, AIEnemy entity, TargetProvider targetProvider) : base(fsm, entity, targetProvider)
    {
    }

    public async override Task OnEnter()
    {
        AIEnemy.AutonomousMover.NavMeshAgent.isStopped = false;
        AIEnemy.AutonomousMover.NavMeshAgent.ResetPath();

        AIEnemy.AutonomousMover.NavMeshAgent.speed = 5.0f;

        await Task.Yield();

        AIEnemy.AutonomousMover.NavMeshAgent.SetDestination(GenerateRandomWaypoint());
    }

    public override void OnFixedUpdate()
    {
        if (AIEnemy.transform.position == AIEnemy.AutonomousMover.NavMeshAgent.pathEndPosition)
            AIEnemy.AutonomousMover.NavMeshAgent.SetDestination(GenerateRandomWaypoint());
    }

    public override void OnUpdate()
    {
        AIEnemy.Animator.Play("Base Layer.Run");
    }

    private Vector3 GenerateRandomWaypoint()
    {
        Vector2 rndPosInsideCircle = UnityEngine.Random.insideUnitCircle * FleeDistance;
        Vector3 rndPos = TargetProvider.Target.position + new Vector3(rndPosInsideCircle.x, 0, rndPosInsideCircle.y);

        if (NavMesh.SamplePosition(rndPos, out NavMeshHit hit, float.PositiveInfinity, NavMesh.AllAreas))
            return hit.position;

        return Vector3.zero;
    }
}
