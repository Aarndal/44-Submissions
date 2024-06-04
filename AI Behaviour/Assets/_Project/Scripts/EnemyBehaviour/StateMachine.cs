using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField]
    private State _currentState;

    //private readonly Dictionary<State, List<Transition>> _transitions;

    private readonly List<State> _states;

    public State CurrentState => _currentState;
    public List<State> States => _states;

    public StateMachine(State initialState)
    {
        _states = new();

        AddState(initialState);
        _currentState = initialState;
    }

    private void Update()
    {
        UpdateState();

        _currentState.OnUpdate();
    }

    private void FixedUpdate()
    {
        _currentState.OnFixedUpdate();
    }

    private void LateUpdate()
    {
        _currentState.OnLateUpdate();
    }

    public void UpdateState()
    {
        State targetState = GetTargetState();

        if (targetState != null && targetState != _currentState)
            TransitionTo(targetState);
    }

    private State GetTargetState()
    {
        foreach (Transition transition in _currentState.Transitions)
        {
            if (transition.Condition() == true)
                return transition.TargetState;
        }

        return null;
    }

    private void TransitionTo(State targetState)
    {
        Debug.Log($"Transitioning from {CurrentState} to {targetState}");

        _currentState.OnExit();
        targetState.OnEnter();
        _currentState = targetState;
    }

    public void AddState(State state)
    {
        if(!_states.Contains(state))
            _states.Add(state);
    }
}
