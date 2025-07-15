namespace Engine.StateManagement;

public class TransitionCondition
{
    public TransitionCondition(Type toStateType, Func<bool> condition)
    {
        ToStateType = toStateType;
        Condition = condition;
    }

    public readonly Type ToStateType;
    public readonly Func<bool> Condition;
}