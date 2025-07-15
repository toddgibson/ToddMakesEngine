namespace Engine.StateManagement;

public class StateChangedEventArgs
{
    public readonly Type currentState;
    public readonly Type previousState;

    public StateChangedEventArgs(Type currentState, Type previousState)
    {
        this.currentState = currentState;
        this.previousState = previousState;
    }
}