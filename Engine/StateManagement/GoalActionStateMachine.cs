namespace Engine.StateManagement;

public class GoalActionStateMachine
{
    // This type of state machine should be passed a goal and then determine the GoalActionStates that
    // must be completed in order to achieve the stated goal...
    // 1. store all available states with goals they accomplish
    // 2. provide a method that takes in a goal and then creates a queue of states to be done in order to meet the goal
    
    private HashSet<GoalActionState> _goalActionStates = [];

    public void AddState(GoalActionState goalActionState) => _goalActionStates.Add(goalActionState);
    public void RemoveState(GoalActionState goalActionState) => _goalActionStates.Remove(goalActionState);

    public Queue<GoalActionState> CreateStateQueueForGoal(byte goal)
    {
        return new Queue<GoalActionState>();
    }
}

public abstract class GoalActionState : State
{
    
}