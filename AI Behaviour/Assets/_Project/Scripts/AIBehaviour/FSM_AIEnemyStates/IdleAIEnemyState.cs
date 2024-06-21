using System;
using UnityEngine;

//[CreateAssetMenu(fileName = "IdleState", menuName = "AI/States/IdleState")]
public sealed class IdleAIEnemyState : AIEnemyState
{
    public event Action<bool> IdleTimeIsUp;

    private bool _timeIsUp;
    private float _timer, _idleTime;

    public bool TimeIsUp
    {
        get => _timeIsUp;
        private set
        {
            if (_timeIsUp != value)
            {
                _timeIsUp = value;
                IdleTimeIsUp?.Invoke(_timeIsUp);
            }
        }
    }

    public IdleAIEnemyState(AIEnemy entity, float idleTime) : base(entity, null)
    {
        _idleTime = idleTime;
        _timer = _idleTime;
    }

    public override void OnEnter()
    {
        AIEnemy.AutonomousMover.NavMeshAgent.isStopped = true;
        _timer = _idleTime;
        TimeIsUp = false;
    }

    public override void OnUpdate()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0)
            TimeIsUp = true;
    }

    public override void OnExit()
    {

    }
}
