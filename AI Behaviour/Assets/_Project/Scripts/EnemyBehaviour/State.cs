using System.Collections.Generic;

public abstract class State
{
    private List<Transition> _transitions;

    public List<Transition> Transitions => _transitions;

    public State()
    {
        _transitions = new List<Transition>();
    }

    public virtual void OnEnter() { }

    public virtual void OnUpdate() { }

    public virtual void OnFixedUpdate() { }

    public virtual void OnLateUpdate() { }

    public virtual void OnExit() { }
}
