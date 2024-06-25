using System;
using UnityEngine;

public class Wolf : FastFightingAIEnemy
{
    public Func<bool> RoamCondition, IdleCondition, ChaseCondition, AttackCondition, CircleCondition;

    [SerializeField]
    private RandomWayPointTargetProvider _wayPointProvider;
    [SerializeField]
    private PlayerTargetProvider _playerProvider;
    [SerializeField]
    private LineOfSightChecker _lineOfSightChecker;

    [Header("Variables")]
    [SerializeField]
    private float _idleTime = 5f;
    [SerializeField]
    private float _roamRadius = 20f;
    [SerializeField]
    private float _attackRange = 2f;


    private StateMachine _myFSM;
    private IdleAIEnemyState _idle;
    private State _roam, _chase, _attack, _circle;
    private Transition _toRoam, _toChase, _toIdle, _toAttack, _toCircle;

    protected override void Awake()
    {
        base.Awake();

        InitializeStates();
        InitializeConditions();
        InitializeTransitions();

        _myFSM = new(_idle);

        _myFSM.AddState(_roam);
        _myFSM.AddState(_chase);
        _myFSM.AddState(_attack);

        _idle.AddTransition(_toRoam);
        _idle.AddTransition(_toChase);

        _roam.AddTransition(_toChase);

        _chase.AddTransition(_toIdle);
        _chase.AddTransition(_toAttack);

        _attack.AddTransition(_toChase);
        _attack.AddTransition(_toCircle);

        _circle.AddTransition(_toChase);
        _circle.AddTransition(_toAttack);
    }

    private void OnEnable()
    {
        //    _idle.IdleTimeIsUp += OnIdleTimeIsUp;
        //    _playerTargetProvider.PlayerInSight += OnPlayerInSight;
    }

    private void Start()
    {
        AutonomousMover.NavMeshAgent.enabled = true;
        Debug.LogWarning("Start State: " + _myFSM.CurrentState);
    }

    private void FixedUpdate()
    {
        _myFSM.OnFixedUpdate();
    }

    private void Update()
    {
        _myFSM.OnUpdate();
    }

    private void LateUpdate()
    {
        _myFSM.OnLateUpdate();
    }

    private void OnDisable()
    {
        //    _playerTargetProvider.PlayerInSight -= OnPlayerInSight;
        //    _idle.IdleTimeIsUp -= OnIdleTimeIsUp;
    }

    //private void OnIdleTimeIsUp(bool ctx) => _toRoam.Condition = () => ctx;
    //private void OnPlayerInSight(bool ctx) => _toChase.Condition = () => ctx;

    private void InitializeStates()
    {
        _idle = new IdleAIEnemyState(this, _idleTime);
        _roam = new RoamAIEnemyState(this, _roamRadius);
        _chase = new ChaseAIEnemyState(this, _playerProvider);
        _attack = new AttackAIEnemyState(this, _playerProvider);
        _circle = new CircleAIEnemyState(this, _playerProvider);
    }

    private void InitializeConditions()
    {
        IdleCondition = () => !_lineOfSightChecker.TargetInSight;
        RoamCondition = () => _idle.TimeIsUp;
        ChaseCondition = () => _lineOfSightChecker.TargetInSight && (_playerProvider.Target.position - this.transform.position).sqrMagnitude > _attackRange * _attackRange;
        //To-Do: Auslagerung in Methode innerhalb von TargetProvider.
        AttackCondition = () => _lineOfSightChecker.TargetInSight && (_playerProvider.Target.position - this.transform.position).sqrMagnitude <= _attackRange * _attackRange;
        CircleCondition = () => false;
    }

    private void InitializeTransitions()
    {
        _toIdle = new Transition("Transition to Idle", IdleCondition, _idle);
        _toRoam = new Transition("Transition to Roam", RoamCondition, _roam);
        _toChase = new Transition("Transition to Chase", ChaseCondition, _chase);
        _toAttack = new Transition("Transition to Attack", AttackCondition, _attack);
        _toCircle = new Transition("Transition to Circle", CircleCondition, _circle);
    }
}
