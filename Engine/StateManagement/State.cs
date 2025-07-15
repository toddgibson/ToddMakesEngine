namespace Engine.StateManagement;

public abstract class State
{
    private StateMachine _stateMachine;

    protected List<TransitionCondition> transitionConditions = [];
    public int TransitionConditionCount => transitionConditions.Count;
    public Type? PreviousStateMachineState => _stateMachine.PreviousStateType;

    public void SetStateMachine(StateMachine stateMachine) => _stateMachine = stateMachine;

    public bool IsEndState { get; set; }

    private bool _timedTickEnabled = false;
    private float _tickTimerSeconds = float.MinValue;
    private Action? TimedTickAction { get; set; }

    /// <summary>
    /// When implementing this method, configure the Transition Conditions
    /// </summary>
    public abstract void Initialize();

    /// <summary>
    /// Called immediately upon changing to this state
    /// </summary>
    public abstract void OnEnter();
    /// <summary>
    /// Called each frame
    /// </summary>
    public abstract void Tick(float deltaTime = 0.0f);
    /// <summary>
    /// Called immediately upon leaving this state
    /// </summary>
    public abstract void OnExit();

    internal void CheckStateTransitions()
    {
        if (_stateMachine.IsInEndState) return;

        var validTransition = transitionConditions.FirstOrDefault(transition => transition.Condition());
        if (validTransition == null) return;

        _stateMachine.ChangeState(_stateMachine.GetStateOfType(validTransition.ToStateType));
    }

    /// <summary>
    /// Call a method after the specified seconds have passed.
    /// </summary>
    /// <param name="seconds">the amount of seconds to elapse before calling the method</param>
    /// <param name="timedTickAction">the method to be called</param>
    protected void SetTimedTick(float seconds, Action timedTickAction)
    {
        _tickTimerSeconds = seconds;
        _timedTickEnabled = true;
        TimedTickAction = timedTickAction;
    }

    internal void UpdateTimer(float deltaTime)
    {
        if (!_timedTickEnabled) return;

        _tickTimerSeconds -= deltaTime;
        if (_tickTimerSeconds > 0.0f) return;

        _timedTickEnabled = false;

        TimedTickAction?.Invoke();
    }

    internal void ClearTimedTickAction()
    {
        TimedTickAction = null;
    }
}