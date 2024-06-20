using System;
using UnityEngine;

[Serializable]
//[CreateAssetMenu(fileName = "NewTransition", menuName = "AI/Transition")]
public class AnyTransition : Transition
{
    public State OriginState { get; private set; }

    public AnyTransition(State originState, State targetState, Func<bool> condition, string name) : base(targetState, condition, name)
    {
        OriginState = originState;
    }
}
