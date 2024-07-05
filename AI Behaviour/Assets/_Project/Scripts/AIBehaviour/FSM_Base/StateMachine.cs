using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    //private readonly Dictionary<State, List<Transition>> _transitions;
    private State _targetState;

    [field: SerializeField]
    public State CurrentState { get; protected set; }
    [field: SerializeField]
    public List<State> States { get; }
    [field: SerializeField]
    public List<Transition> AnyTransitions { get; }

    public StateMachine(State initialState)
    {
        States = new();
        AnyTransitions = new();

        AddState(initialState);
        CurrentState = initialState;
    }

    public void OnFixedUpdate()
    {
        CurrentState.OnFixedUpdate();
    }

    public void OnUpdate()
    {
        SwitchState();

        CurrentState.OnUpdate();
    }

    public void OnLateUpdate()
    {
        CurrentState.OnLateUpdate();
    }

    public void SwitchState()
    {
        _targetState = GetTargetState();

        if (_targetState != null && _targetState != CurrentState)
            TransitionTo(_targetState);
    }

    private State GetTargetState()
    {
        foreach (Transition transition in AnyTransitions)
        {
            if (transition.Condition() == true)
                return transition.TargetState;
        }

        foreach (Transition transition in CurrentState.Transitions)
        {
            if (transition.Condition() == true)
                return transition.TargetState;
        }

        return null;
    }

    private void TransitionTo(State targetState)
    {
        Debug.LogWarning($"Transitioning from {CurrentState} to {targetState}");

        CurrentState.OnExit();
        targetState.OnEnter();
        CurrentState = targetState;
    }

    public void AddState(State state)
    {
        if (!States.Contains(state))
            States.Add(state);
    }

    public void AddAnyTransition(Transition transition)
    {
        if (!AnyTransitions.Contains(transition))
            AnyTransitions.Add(transition);
    }

    public void SetCurrentState(State state)
    {
        CurrentState = state;
    }
}
