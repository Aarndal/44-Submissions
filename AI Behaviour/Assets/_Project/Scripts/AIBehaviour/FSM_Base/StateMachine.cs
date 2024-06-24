using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    //private readonly Dictionary<State, List<Transition>> _transitions;
    [SerializeField]
    private State _currentState;
    [SerializeField]
    private List<State> _states;
    [SerializeField]
    private List<Transition> _anyTransitions;

    private State _targetState;

    public List<State> States => _states;
    public List<Transition> AnyTransitions => _anyTransitions;
    public State CurrentState => _currentState;

    public StateMachine(State initialState)
    {
        _states = new();
        _anyTransitions = new();

        AddState(initialState);
        _currentState = initialState;
    }

    public void OnFixedUpdate()
    {
        _currentState.OnFixedUpdate();
    }

    public void OnUpdate()
    {
        SwitchState();

        _currentState.OnUpdate();
    }

    public void OnLateUpdate()
    {
        _currentState.OnLateUpdate();
    }

    public void SwitchState()
    {
        _targetState = GetTargetState();

        if (_targetState != null && _targetState != _currentState)
            TransitionTo(_targetState);
    }

    private State GetTargetState()
    {
        foreach (Transition transition in _anyTransitions)
        {
            if (transition.Condition() == true)
                return transition.TargetState;
        }

        foreach (Transition transition in _currentState.Transitions)
        {
            if (transition.Condition() == true)
                return transition.TargetState;
        }

        return null;
    }

    private void TransitionTo(State targetState)
    {
        Debug.LogWarning($"Transitioning from {CurrentState} to {targetState}");

        _currentState.OnExit();
        targetState.OnEnter();
        _currentState = targetState;
    }

    public void AddState(State state)
    {
        if (!_states.Contains(state))
            _states.Add(state);
    }

    public void AddAnyTransition(Transition transition)
    {
        if (!_anyTransitions.Contains(transition))
            _anyTransitions.Add(transition);
    }

    public void SetCurrentState(State state)
    {
        _currentState = state;
    }
}
