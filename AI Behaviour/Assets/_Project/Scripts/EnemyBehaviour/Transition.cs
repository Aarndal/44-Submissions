
public abstract class Transition
{
    private State _targetState;

    public State TargetState => _targetState;

    public Transition(State targetState)
    {
        _targetState = targetState;
    }

    public abstract bool Condition();
}
