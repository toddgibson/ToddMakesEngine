namespace Engine.StateManagement
{
    public class StateMachine
    {
        private List<State> _states = [];
        private List<TransitionCondition> _anyStateTransitionConditions = [];

        private State? _currentState;

        public Type? PreviousStateType { get; private set; }

        public delegate void StateChangedEventHandler(object sender, StateChangedEventArgs e);
        public event StateChangedEventHandler? OnStateChanged;

        public bool IsInEndState => _currentState?.IsEndState ?? false;

        public void Initialize(List<State> states, List<TransitionCondition> anyStateTransitionConditions)
        {
            if (!states.Any()) return;

            _states = states;
            _states.ForEach(state =>
            {
                state.SetStateMachine(this);
                state.Initialize();

                if (!state.IsEndState && state.TransitionConditionCount == 0)
                    throw new InvalidOperationException($"State {state.GetType()} has no configured transitions. Configure them in the {nameof(Initialize)} method of your state implementation.");
            });
            _anyStateTransitionConditions = anyStateTransitionConditions;

            _currentState = states[0];
            _currentState.OnEnter();
        }

        protected internal void ChangeState(State newState)
        {
            var previousState = _currentState;

            if (previousState != null)
            {
                PreviousStateType = previousState.GetType();
                previousState.ClearTimedTickAction();
                previousState.OnExit();
            }

            _currentState = newState;
            _currentState.OnEnter();

            OnStateChanged?.Invoke(this, new StateChangedEventArgs(_currentState.GetType(), previousState!.GetType()));
        }

        public void Tick(float deltaTime = 0.0f)
        {
            if (_currentState == null) return;

            _currentState.Tick(deltaTime);
            _currentState.UpdateTimer(deltaTime);
            _currentState.CheckStateTransitions();
            CheckAnyStateTransitions();
        }

        private void CheckAnyStateTransitions()
        {
            if (_currentState == null || _currentState.IsEndState) return;

            var validTransition = _anyStateTransitionConditions.FirstOrDefault(transition => transition.Condition());
            if (validTransition == null) return;

            ChangeState(GetStateOfType(validTransition.ToStateType));
        }

        public State GetStateOfType(Type toStateType)
        {
            var nextState = _states.FirstOrDefault(p => p.GetType() == toStateType);
            if (nextState == null)
                throw new InvalidOperationException($"Invalid Transition! State machine does not have a state of type {toStateType}");
            return nextState;
        }
    }
}