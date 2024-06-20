
//[CreateAssetMenu(fileName = "RoamState", menuName = "AI/States/RoamState")]
public class RoamState : AIEnemyState
{
    public RoamState(AIEnemy entity) : base(entity, null)
    {
    }

    public override void OnEnter()
    {
        AIEnemy.AutonomousMover.NavMeshAgent.isStopped = false;
    }
}
