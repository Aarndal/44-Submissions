using System;
using UnityEngine;

//[CreateAssetMenu(fileName = "IdleState", menuName = "AI/States/IdleState")]
public class IdleState : AIEnemyState
{
    public event Action<bool> IdleTimeIsUp;

    private float _timer, _idleTime;

    public bool TimeIsUp { get; private set; }

    public IdleState(AIEnemy entity, float idleTime) : base(entity, null)
    {
        _idleTime = idleTime;
        _timer = _idleTime;
    }

    public override void OnEnter()
    {
        _timer = _idleTime; 
        TimeIsUp = false;
        IdleTimeIsUp?.Invoke(false);
        AIEnemy.AutonomousMover.NavMeshAgent.isStopped = true;
    }

    public override void OnUpdate()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0)
        {
            TimeIsUp = true;
            IdleTimeIsUp?.Invoke(true);
        }
    }

    public override void OnExit()
    {
        
    }
}
