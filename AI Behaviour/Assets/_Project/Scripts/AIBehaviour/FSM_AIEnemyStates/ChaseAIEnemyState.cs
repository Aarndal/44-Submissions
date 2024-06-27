using System.Collections;
using UnityEngine;

//[CreateAssetMenu(fileName = "ChaseState", menuName = "AI/States/ChaseState")]
public sealed class ChaseAIEnemyState : AIEnemyState
{
    private float _prevStoppingDistance;

    public ChaseAIEnemyState(AIEnemy entity, TargetProvider targetProvider, LineOfSightChecker sightChecker) : base(entity, targetProvider)
    {
    }

    public override void OnEnter()
    {
        AIEnemy.AutonomousMover.NavMeshAgent.autoBraking = false;

        AIEnemy.AutonomousMover.NavMeshAgent.isStopped = false;
        AIEnemy.AutonomousMover.NavMeshAgent.ResetPath();

        AIEnemy.AutonomousMover.NavMeshAgent.speed = 5.0f;
        
        AIEnemy.Animator.Play("Base Layer.Howl");
    }

    public override void OnFixedUpdate()
    {
        if (AIEnemy.Animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Run"))
            AIEnemy.AutonomousMover.MoveTo(TargetProvider);
    }

    public override void OnUpdate()
    {
        AIEnemy.Animator.SetBool("HasHowled", true);
    }

    public override void OnExit()
    {
        AIEnemy.AutonomousMover.NavMeshAgent.autoBraking = true;
    }
}
