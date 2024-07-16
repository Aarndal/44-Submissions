using System;
using System.Threading.Tasks;
using UnityEngine;

//[CreateAssetMenu(fileName = "IdleState", menuName = "AI/States/IdleState")]
public sealed class IdleAIEnemyState : AIEnemyState
{
    public event Action<bool> IdleTimeIsUp;

    public readonly float MinIdleTime = 5.0f;

    private bool _timeIsUp = false;
    private float _timer = 0f, _idleTime = 0f, _animationLoopCount = 0f;

    public float IdleTime
    {
        get => _idleTime < MinIdleTime ? MinIdleTime : _idleTime;
        set
        {
            if (_idleTime != value)
                _idleTime = value;

            if (_idleTime < MinIdleTime)
            {
                _idleTime = MinIdleTime;
                Debug.LogFormat("IdelTime has been set to MinIdleTime.");
            }
        }
    }

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

    public IdleAIEnemyState(StateMachine fsm, AIEnemy entity) : base(fsm, entity, null)
    {
        _timer = IdleTime;
    }

    public async override Task OnEnter()
    {
        _timer = IdleTime;
        TimeIsUp = false;

        AIEnemy.AutonomousMover.NavMeshAgent.isStopped = true;
        AIEnemy.AutonomousMover.NavMeshAgent.ResetPath();

        await Task.Yield();

        AIEnemy.Animator.Play("Base Layer.Idle");
    }

    public override void OnUpdate()
    {
        //------------------
        //https://github.com/llamacademy/ai-series-part-47/blob/main/Assets/Scripts/FSM/States/IdleState.cs

        AnimatorStateInfo currentAnimatorState = AIEnemy.Animator.GetCurrentAnimatorStateInfo(0);

        if (currentAnimatorState.normalizedTime >= _animationLoopCount + 1)
        {
            float value = UnityEngine.Random.value;
            if (value < 0.7f)
            {
                if (!currentAnimatorState.IsName("Base Layer.Idle"))
                    _animationLoopCount = 0;
                else
                    _animationLoopCount++;

                AIEnemy.Animator.Play("Base Layer.Idle");
            }
            else if (value < 0.9f)
            {
                if (!currentAnimatorState.IsName("Base Layer.Sit"))
                    _animationLoopCount = 0;
                else
                    _animationLoopCount++;

                AIEnemy.Animator.Play("Base Layer.Sit");
            }
            else
            {
                if (!currentAnimatorState.IsName("Base Layer.Dig"))
                    _animationLoopCount = 0;
                else
                    _animationLoopCount++;

                AIEnemy.Animator.Play("Base Layer.Dig");
            }
        }
        //------------------

        _timer -= Time.deltaTime;

        if (_timer <= 0)
            TimeIsUp = true;
    }

    public async override Task OnExit()
    {
        _timer = IdleTime;

        await Task.Yield();

        if (TimeIsUp)
            TimeIsUp = false;
    }
}
