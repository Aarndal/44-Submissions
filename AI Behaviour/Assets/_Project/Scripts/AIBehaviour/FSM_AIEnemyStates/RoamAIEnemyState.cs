using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

//[CreateAssetMenu(fileName = "RoamState", menuName = "AI/States/RoamState")]
public sealed class RoamAIEnemyState : AIEnemyState
{
    private float _prevStoppingDistance;

    public float Radius { get; private set; }

    public RoamAIEnemyState(StateMachine fsm, AIEnemy entity, float radius) : base(fsm, entity, null)
    {
        Radius = radius;
    }

    public async override Task OnEnter()
    {
        AIEnemy.AutonomousMover.NavMeshAgent.isStopped = false;
        AIEnemy.AutonomousMover.NavMeshAgent.autoRepath = true;
        AIEnemy.AutonomousMover.NavMeshAgent.autoBraking = true;

        _prevStoppingDistance = AIEnemy.AutonomousMover.MinDistanceToTarget;
        AIEnemy.AutonomousMover.MinDistanceToTarget = 0.5f;

        AIEnemy.AutonomousMover.NavMeshAgent.ResetPath();

        AIEnemy.AutonomousMover.NavMeshAgent.speed = 2.0f;

        await Task.Yield();

        AIEnemy.AutonomousMover.NavMeshAgent.SetDestination(GenerateRandomWaypoint());
    }

    public async override void OnFixedUpdate()
    {
        if (AIEnemy.AutonomousMover.ReachedTarget || AIEnemy.AutonomousMover.NavMeshAgent.isPathStale || !AIEnemy.AutonomousMover.NavMeshAgent.hasPath)
        {
            Debug.LogWarning($"DistanceToTarget: {AIEnemy.AutonomousMover.DistanceToTarget} | StoppingDistance: {AIEnemy.AutonomousMover.MinDistanceToTarget}");
            AIEnemy.AutonomousMover.NavMeshAgent.SetDestination(GenerateRandomWaypoint());
            await Task.Yield();
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

    private Vector3 GenerateRandomWaypoint()
    {
        Vector2 rndPosInsideCircle = UnityEngine.Random.insideUnitCircle * Radius;
        Vector3 rndPos = AIEnemy.AutonomousMover.InitialPosition + new Vector3(rndPosInsideCircle.x, 0, rndPosInsideCircle.y);

        if (NavMesh.SamplePosition(rndPos, out NavMeshHit hit, float.PositiveInfinity, NavMesh.AllAreas))
            return hit.position;

        return AIEnemy.AutonomousMover.InitialPosition;
    }
}
