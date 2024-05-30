using System.Collections.Generic;

public abstract class State
{
    private List<Transition> _transitions;
    
    public List<Transition> Transitions => _transitions;

    public State()
    {
        _transitions = new ();
    }

    public virtual void OnEnter() { }

    public virtual void OnUpdate() { }

    public virtual void OnFixedUpdate() { }

    public virtual void OnLateUpdate() { }

    public virtual void OnExit() { }

    public void AddTransition(Transition transition)
    {
        if (!_transitions.Contains(transition))
            _transitions.Add(transition);
    }
}
