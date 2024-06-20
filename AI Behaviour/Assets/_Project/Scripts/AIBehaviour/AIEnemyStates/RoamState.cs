using UnityEngine;
using UnityEngine.AI;

//[CreateAssetMenu(fileName = "RoamState", menuName = "AI/States/RoamState")]
public sealed class RoamState : AIEnemyState
{
    public float Radius { get; private set; }

    public RoamState(AIEnemy entity, float radius) : base(entity, null)
    {
        Radius = radius;
    }

    public override void OnEnter()
    {
        AIEnemy.AutonomousMover.NavMeshAgent.enabled = true;
        AIEnemy.AutonomousMover.NavMeshAgent.isStopped = false;
        AIEnemy.AutonomousMover.NavMeshAgent.SetDestination(GenerateRandomWaypoint(Radius));
    }

    public override void OnFixedUpdate()
    {
        if (AIEnemy.AutonomousMover.NavMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
            AIEnemy.AutonomousMover.NavMeshAgent.SetDestination(GenerateRandomWaypoint(Radius));
    }

    public override void OnExit()
    {

    }

    private Vector3 GenerateRandomWaypoint(float radius)
    {
        Vector2 rndPosInsideCircle = UnityEngine.Random.insideUnitCircle * radius;
        Vector3 rndPos = AIEnemy.AutonomousMover.CurrentPosition + new Vector3(rndPosInsideCircle.x, 0, rndPosInsideCircle.y);

        if (NavMesh.SamplePosition(rndPos, out NavMeshHit hit, float.PositiveInfinity, NavMesh.AllAreas))
            return hit.position;

        return Vector3.zero;
    }
}
