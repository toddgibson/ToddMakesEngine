namespace Engine.StateManagement;

public class StateChangedEventArgs(Type currentState, Type previousState)
{
    public readonly Type CurrentState = currentState;
    public readonly Type PreviousState = previousState;
}