using System;
using UnityEngine;

[Serializable]
//[CreateAssetMenu(fileName = "NewTransition", menuName = "AI/Transition")]
public class Transition
{
    [SerializeField]
    private string _name;
    [SerializeField]
    private State _targetState;

    private Func<bool> _condition;

    public string Name { get => _name; private set => _name = value; }
    public State OriginState { get; private set; }
    public State TargetState { get => _targetState; private set => _targetState = value; }
    public Func<bool> Condition { get => _condition; set => _condition = value; }

    public Transition(string name, Func<bool> condition, State targetState, State originState = null)
    {
        OriginState = originState;
        TargetState = targetState;
        Condition = condition;
        Name = name;
    }
}
