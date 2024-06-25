using System.Collections;
using UnityEngine;

//[CreateAssetMenu(fileName = "ChaseState", menuName = "AI/States/ChaseState")]
public sealed class ChaseAIEnemyState : AIEnemyState
{
    public ChaseAIEnemyState(AIEnemy entity, TargetProvider targetProvider) : base(entity, targetProvider)
    {
    }

    public override void OnEnter()
    {
        AIEnemy.AutonomousMover.NavMeshAgent.isStopped = false;
        AIEnemy.AutonomousMover.NavMeshAgent.speed = 5.0f;
        AIEnemy.Animator.Play("Base Layer.Howl");
    }

    public override void OnUpdate()
    {
        AIEnemy.Animator.SetBool("HasHowled", true);

        if (AIEnemy.Animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Run"))
            AIEnemy.AutonomousMover.MoveTo(TargetProvider);
    }
}
