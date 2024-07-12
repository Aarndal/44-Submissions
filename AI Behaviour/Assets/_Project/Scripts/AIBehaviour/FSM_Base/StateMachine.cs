using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class StateMachine
{
    private bool _isTransitioning = false;

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


    public StateMachine()
    {
        States = new();
        AnyTransitions = new();
        History = new();
    }

    #region Unity Build-In Callbacks
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
    #endregion

    #region Private Methods
    private async void SwitchState()
    {
        _targetState = GetTargetState();

        if (_targetState != null && _targetState != CurrentState && !_isTransitioning)
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
        _isTransitioning = true;

        if (!States.Contains(targetState))
            States.Add(targetState);

        if (!History.Contains(targetState))
            History.Push(targetState);

        //History length checken und gegebenfalls untersten State entfernen

        Debug.LogWarning($"Transitioning from {CurrentState} to {targetState}");

        if (CurrentState != null)
            await CurrentState.OnExit();

        await targetState.OnEnter();
        CurrentState = targetState;

        _isTransitioning = false;
    }
    #endregion

    #region Public Methods
    public void AddState(State state)
    {
        States.Add(state);
    }

    public void AddAnyTransition(Transition transition)
    {
        if (!AnyTransitions.Contains(transition))
            AnyTransitions.Add(transition);
    }

    public void SetInitialState(State state)
    {
        States.Add(state);

        History.Push(state);
        CurrentState = state;
    }
    #endregion
}
