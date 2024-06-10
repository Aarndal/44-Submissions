using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class State
{
    [SerializeField]
    protected List<Transition> _transitions;

    protected Entity _entity;

    public List<Transition> Transitions => _transitions;

    public State(Entity entity)
    {
        _entity = entity;
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
