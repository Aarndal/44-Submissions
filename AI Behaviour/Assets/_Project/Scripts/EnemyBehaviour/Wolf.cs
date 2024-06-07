using System;
using UnityEngine;

public class Wolf : Enemy
{
    private PlayerTargetProvider _targetProvider;

    [SerializeField]
    private float _timer;
    [SerializeField]
    private float _idleTime = 10f;

    private StateMachine _myFSM;
    private State _idle, _roam, _chase;
    private Transition _toRoam, _toChase, _toIdle;
    public static Func<bool> RoamCondition, IdleCondition, ChaseCondition;

    private void Awake()
    {
        base.Awake();
        _targetProvider = GetComponent<PlayerTargetProvider>();
    }

    private void OnEnable()
    {
        RoamCondition = () => _timer <= 0;
        ChaseCondition = () => _targetProvider.HasTarget;
    }

    private void Start()
    {
        InitializeStates();

        InitializeTransitions();

        _myFSM = new(_idle);

        _myFSM.AddState(_roam);
        _myFSM.AddState(_chase);

        _idle.AddTransition(_toRoam);
        _roam.AddTransition(_toChase);
        _chase.AddTransition(_toIdle);

        _timer = _idleTime;
    }

    private void Update()
    {
        _myFSM.OnUpdate();
        _timer -= Time.deltaTime;
    }

    private void InitializeStates()
    {
        _idle = new IdleState(this, AutonomousMover);
        _roam = new RoamState(this, AutonomousMover);
        _chase = new ChaseState(this, AutonomousMover);
    }

    private void InitializeTransitions()
    {
        _toRoam = new Transition(_roam, RoamCondition, "Transition to Roam");
        _toChase = new Transition(_chase, ChaseCondition, "Transition to Chase");
        _toIdle = new Transition(_idle, IdleCondition, "Transition to Idle");
    }
}
