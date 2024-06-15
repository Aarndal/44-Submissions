using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    //private readonly Dictionary<State, List<Transition>> _transitions;
    [SerializeField]
    private List<State> _states;
    [SerializeField]
    private State _currentState;

    public List<State> States => _states;
    public State CurrentState => _currentState;

    public StateMachine(State initialState)
    {
        _states = new();

        AddState(initialState);
        _currentState = initialState;
    }

    public void OnUpdate()
    {
        UpdateState();

        _currentState.OnUpdate();
    }

    public void OnFixedUpdate()
    {
        _currentState.OnFixedUpdate();
    }

    public void OnLateUpdate()
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
        Debug.LogWarning($"Transitioning from {CurrentState} to {targetState}");

        _currentState.OnExit();
        targetState.OnEnter();
        _currentState = targetState;
    }

    public void AddState(State state)
    {
        if(!_states.Contains(state))
            _states.Add(state);
    }

    //public void SetCurrentState(State state)
    //{
    //   _currentState = state;
    //}
}
