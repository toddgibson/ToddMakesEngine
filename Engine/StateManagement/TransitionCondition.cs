namespace Engine.StateManagement;

public class TransitionCondition(Type toStateType, Func<bool> condition)
{
    public readonly Type ToStateType = toStateType;
    public readonly Func<bool> Condition = condition;
}