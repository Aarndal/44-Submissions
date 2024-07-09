using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class SearchAIEnemyState : AIEnemyState
{
    private Vector3 _currentTargetPos;

    private Vector3[] _tempCorners = new Vector3[10];
    private NavMeshPath _tempNavMeshPath;

    public bool LostTarget { get; private set; }

    public SearchAIEnemyState(StateMachine fsm, AIEnemy entity, TargetProvider targetProvider) : base(fsm, entity, targetProvider) { }

    public override void OnEnter()
    {
        LostTarget = false;
        
        AIEnemy.AutonomousMover.NavMeshAgent.autoBraking = false;

        AIEnemy.AutonomousMover.NavMeshAgent.isStopped = false;
        AIEnemy.AutonomousMover.NavMeshAgent.ResetPath();

        AIEnemy.AutonomousMover.NavMeshAgent.speed = 5.0f;

        _currentTargetPos = TargetProvider.Target.position;

        AIEnemy.Animator.Play("Base Layer.Run");
    }

    public override void OnUpdate()
    {
        SearchForTarget();
    }

    public override void OnExit()
    {
        //LostTarget = false;
    }

    private async void SearchForTarget()
    {
        AIEnemy.AutonomousMover.NavMeshAgent.SetDestination(_currentTargetPos);
        await SearchLastKnownPosition();

        _currentTargetPos = TargetProvider.Target.position;
        await SearchFirstCornerOfCurrenPosition();
    }

    private async Task SearchLastKnownPosition()
    {
        //while (AIEnemy.transform.position != AIEnemy.AutonomousMover.NavMeshAgent.pathEndPosition || AIEnemy.AutonomousMover.DistanceToTarget > 0.5f)
        //    await Task.Yield();

        if (!AIEnemy.AutonomousMover.NavMeshAgent.pathPending)
        {
            if (AIEnemy.AutonomousMover.DistanceToTarget <= AIEnemy.AutonomousMover.MinDistanceToTarget)
            {
                if (!AIEnemy.AutonomousMover.NavMeshAgent.hasPath || AIEnemy.AutonomousMover.NavMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    await Task.Yield();
                }
            }
        }

    }

    private async Task SearchFirstCornerOfCurrenPosition()
    {
        bool hasPathToTarget = NavMesh.CalculatePath(AIEnemy.transform.position, _currentTargetPos, AIEnemy.AutonomousMover.NavMeshAgent.areaMask, _tempNavMeshPath);

        if (!hasPathToTarget)
            LostTarget = true;

        //_tempNavMeshPath.GetCornersNonAlloc(_tempCorners);

        AIEnemy.AutonomousMover.NavMeshAgent.SetDestination(_tempNavMeshPath.corners[1]);

        //while (AIEnemy.transform.position != AIEnemy.AutonomousMover.NavMeshAgent.pathEndPosition || AIEnemy.AutonomousMover.DistanceToTarget > 0.5f)
        //    await Task.Yield();

        if (!AIEnemy.AutonomousMover.NavMeshAgent.pathPending)
        {
            if (AIEnemy.AutonomousMover.DistanceToTarget <= AIEnemy.AutonomousMover.MinDistanceToTarget)
            {
                if (!AIEnemy.AutonomousMover.NavMeshAgent.hasPath || AIEnemy.AutonomousMover.NavMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    await Task.Yield();
                }
            }
        }

        LostTarget = true;
    }
}
