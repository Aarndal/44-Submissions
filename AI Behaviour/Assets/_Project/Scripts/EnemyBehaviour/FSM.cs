using UnityEngine;

public class FSM : MonoBehaviour
{
    [SerializeField]
    private State _currentState;

    public FSM(State startState)
    {
        _currentState = startState;
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
            if (transition.Condition())
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
