using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField]
    private State _currentState;

    private List<State> _states;

    public List<State> States => _states;

    public StateMachine(State startState)
    {
        _currentState = startState;
        _states.Add(_currentState);
    }

    private void Update()
    {
        CheckState();

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

    public void CheckState()
    {
        State nextState = GetNextState();

        if (nextState != null && nextState != _currentState)
            SwitchState(nextState);
    }

    private State GetNextState()
    {
        foreach (Transition transition in _currentState.Transitions)
        {
            if (transition.Condition() == true)
                return transition.TargetState;
        }

        return null;
    }

    private void SwitchState(State nextState)
    {
        _currentState.OnExit();
        _currentState = nextState;
        _currentState.OnEnter();
    }
}
