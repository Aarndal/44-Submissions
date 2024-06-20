using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class State
{
    public Entity Entity { get; protected set; }
    public List<Transition> Transitions { get; protected set; }

    public State(Entity entity)
    {
        Entity = entity;
        Transitions = new();
    }

    public virtual void OnEnter() { }

    public virtual void OnFixedUpdate() { }

    public virtual void OnUpdate() { }

    public virtual void OnLateUpdate() { }

    public virtual void OnExit() { }

    public void AddTransition(Transition transition)
    {
        if (!Transitions.Contains(transition))
            Transitions.Add(transition);
    }
}
