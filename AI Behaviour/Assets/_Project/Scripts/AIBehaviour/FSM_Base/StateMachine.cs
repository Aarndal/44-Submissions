using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class StateMachine
{
    private State _currentState;
    private State _targetState;

    public State CurrentState
    {
        get => _currentState;
        protected set
        {
            if (States.Contains(value) && _currentState != value)
                _currentState = value;
        }
    }

    public HashSet<State> States { get; }
    public List<Transition> AnyTransitions { get; }
    public Stack<State> History { get; }


    public StateMachine(State initialState)
    {
        States = new();
        AnyTransitions = new();
        History = new();

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

    public async void SwitchState()
    {
        _targetState = GetTargetState();

        if (_targetState != null && _targetState != CurrentState)
            await TransitionTo(_targetState);
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

    private async Task TransitionTo(State targetState)
    {
        Debug.LogWarning($"Transitioning from {CurrentState} to {targetState}");

        if(!States.Contains(targetState))
            States.Add(targetState);

        if (!History.Contains(targetState))
            History.Push(targetState);

        if (CurrentState != null)
            await CurrentState.OnExit();

        await targetState.OnEnter();
        CurrentState = targetState;
    }

    public void AddState(State state)
    {
        States.Add(state);
    }

    public void AddAnyTransition(Transition transition)
    {
        if (!AnyTransitions.Contains(transition))
            AnyTransitions.Add(transition);
    }
}
