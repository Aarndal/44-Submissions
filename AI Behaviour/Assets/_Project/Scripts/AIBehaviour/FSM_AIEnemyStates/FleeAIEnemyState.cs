

using System;
using UnityEngine;
using UnityEngine.AI;

public class FleeAIEnemyState : AIEnemyState
{
    public FleeAIEnemyState(AIEnemy entity, TargetProvider targetProvider) : base(entity, targetProvider)
    {
    }

    public override void OnEnter()
    {
        AIEnemy.AutonomousMover.NavMeshAgent.isStopped = false;
        AIEnemy.AutonomousMover.NavMeshAgent.speed = 5.0f;
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
        Vector2 rndPosInsideCircle = UnityEngine.Random.insideUnitCircle * 100f;
        Vector3 rndPos = TargetProvider.Target.position + new Vector3(rndPosInsideCircle.x, 0, rndPosInsideCircle.y);

        if (NavMesh.SamplePosition(rndPos, out NavMeshHit hit, float.PositiveInfinity, NavMesh.AllAreas))
            return hit.position;

        return Vector3.zero;
    }
}
