using System;

public class Transition
{
    public string Name { get; private set; }
    public StateMachine StateMachine { get; private set; }
    public State TargetState { get; private set; }
    public Func<bool> Condition { get; private set; }

    public Transition(StateMachine stateMachine, State targetState, Func<bool> condition, string name)
    {
        StateMachine = stateMachine;
        TargetState = targetState;
        Condition = condition;
        Name = name;
    }
}
