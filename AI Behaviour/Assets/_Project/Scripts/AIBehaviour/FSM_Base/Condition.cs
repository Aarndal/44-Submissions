using System;

public class Condition : IPredicate
{
    readonly Func<bool> func;

    public Condition(Func<bool> func)
    {
        this.func = func;
    }

    public bool IsMet() => func.Invoke();
}