using System;

public class Condition : IPredicate
{
    public Func<bool> Event;

    public Condition(Func<bool> @event)
    {
        this.Event = @event;
    }

    public bool IsMet() => Event.Invoke();
}