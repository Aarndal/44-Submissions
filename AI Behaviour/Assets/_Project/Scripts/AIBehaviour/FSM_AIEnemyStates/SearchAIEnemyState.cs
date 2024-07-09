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

    public async override Task OnEnter()
    {
        AIEnemy.AutonomousMover.NavMeshAgent.autoBraking = false;

        AIEnemy.AutonomousMover.NavMeshAgent.isStopped = false;
        AIEnemy.AutonomousMover.NavMeshAgent.ResetPath();

        AIEnemy.AutonomousMover.NavMeshAgent.speed = 5.0f;

        _currentTargetPos = TargetProvider.Target.position;
        
        await Task.Yield();
        
        AIEnemy.Animator.Play("Base Layer.Run");
    }

    public override void OnFixedUpdate()
    {
        SearchForTarget();
    }

    private async void SearchForTarget()
    {
        await SearchLastKnownPosition(_currentTargetPos);

        _currentTargetPos = TargetProvider.Target.position;

        await SearchFirstCornerOfCurrenPosition(_currentTargetPos);
    }

    private async Task SearchLastKnownPosition(Vector3 pos)
    {
        AIEnemy.AutonomousMover.NavMeshAgent.SetDestination(pos);

        while (AIEnemy.transform.position != AIEnemy.AutonomousMover.NavMeshAgent.pathEndPosition || AIEnemy.AutonomousMover.DistanceToTarget > 0.5f)
            await Task.Yield();
    }

    private async Task SearchFirstCornerOfCurrenPosition(Vector3 pos)
    {
        bool hasPathToTarget = !NavMesh.CalculatePath(AIEnemy.transform.position, pos, AIEnemy.AutonomousMover.NavMeshAgent.areaMask, _tempNavMeshPath);

        if(!hasPathToTarget)
            LostTarget = true;

        //_tempNavMeshPath.GetCornersNonAlloc(_tempCorners);

        AIEnemy.AutonomousMover.NavMeshAgent.SetDestination(_tempNavMeshPath.corners[1]);

        while (AIEnemy.transform.position != AIEnemy.AutonomousMover.NavMeshAgent.pathEndPosition || AIEnemy.AutonomousMover.DistanceToTarget > 0.5f)
            await Task.Yield();

        LostTarget = true;
    }
}
