using System;

public struct Transition
{
    public string Name { get; private set; }
    //public StateMachine StateMachine { get; private set; }
    public State TargetState { get; private set; }
    public Func<bool> Condition { get; private set; }

    public Transition(State targetState, Func<bool> condition, string name)
    {
        TargetState = targetState;
        Condition = condition;
        Name = name;
    }
}
