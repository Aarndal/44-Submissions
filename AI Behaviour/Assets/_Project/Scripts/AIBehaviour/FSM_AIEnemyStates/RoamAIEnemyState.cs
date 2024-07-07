using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

//[CreateAssetMenu(fileName = "RoamState", menuName = "AI/States/RoamState")]
public sealed class RoamAIEnemyState : AIEnemyState
{
    public float Radius { get; private set; }

    public RoamAIEnemyState(AIEnemy entity, float radius) : base(entity, null)
    {
        Radius = radius;
    }

    public async override Task OnEnter()
    {
        AIEnemy.AutonomousMover.NavMeshAgent.isStopped = false;
        AIEnemy.AutonomousMover.NavMeshAgent.stoppingDistance = 0.0f;
        AIEnemy.AutonomousMover.NavMeshAgent.ResetPath();

        AIEnemy.AutonomousMover.NavMeshAgent.speed = 2.0f;

        await Task.Yield();

        AIEnemy.AutonomousMover.NavMeshAgent.SetDestination(GenerateRandomWaypoint());
    }

    public override void OnFixedUpdate()
    {
        if (AIEnemy.transform.position == AIEnemy.AutonomousMover.NavMeshAgent.pathEndPosition || AIEnemy.AutonomousMover.DistanceToTarget <= 0.5f)
            AIEnemy.AutonomousMover.NavMeshAgent.SetDestination(GenerateRandomWaypoint());
    }

    public override void OnUpdate()
    {
        AIEnemy.Animator.Play("Base Layer.Walk");
    }

    private Vector3 GenerateRandomWaypoint()
    {
        Vector2 rndPosInsideCircle = UnityEngine.Random.insideUnitCircle * Radius;
        Vector3 rndPos = AIEnemy.AutonomousMover.InitialPosition + new Vector3(rndPosInsideCircle.x, 0, rndPosInsideCircle.y);

        if (NavMesh.SamplePosition(rndPos, out NavMeshHit hit, float.PositiveInfinity, NavMesh.AllAreas))
            return hit.position;

        return Vector3.zero;
    }
}
