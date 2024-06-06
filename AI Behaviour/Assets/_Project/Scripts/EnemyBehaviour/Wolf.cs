using System;
using UnityEngine;

public class Wolf : Enemy
{
    private PlayerTargetProvider _targetProvider;

    [SerializeField]
    private float _timer;
    private float _idleTime = 5f;

    private StateMachine _myFSM;

    public static Func<bool> RoamCondition;
    public static Func<bool> ChaseCondition;
    public static Func<bool> IdleCondition;

    private void Awake()
    {
        _targetProvider = GetComponent<PlayerTargetProvider>();
    }

    private void OnEnable()
    {
        RoamCondition = () => _timer <= 0;
        ChaseCondition = () => _targetProvider.HasTarget;
    }

    private void Start()
    {
        _timer = _idleTime;
        
        State idle = new IdleState(this, AutonomousMover);
        State roam = new RoamState(this, AutonomousMover);
        State chase = new ChaseState(this, AutonomousMover);

        Transition transitionToRoam = new Transition(roam, RoamCondition, "Transition to Roam");
        Transition transitionToChase = new Transition(chase, ChaseCondition, "Transition to Chase");
        Transition transitionToIdle = new Transition(idle, IdleCondition, "Transition to Idle");

        _myFSM = new StateMachine(idle);
        
        idle.AddTransition(transitionToRoam);
        roam.AddTransition(transitionToChase);
        chase.AddTransition(transitionToIdle);

        _myFSM.AddState(idle);
        _myFSM.AddState(roam);
        _myFSM.AddState(chase);
    }

    private void Update()
    {
        _timer -= Time.deltaTime;

        _myFSM.OnUpdate();
    }

}
