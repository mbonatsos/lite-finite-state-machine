using System;

namespace FiniteStateMachine
{
    public sealed class StateMachine
    {
        private readonly IState[] _states;
        private IState _currentState;
        public IState CurrentState => _currentState;
        private int _index;

        public StateMachine(int stateCount)
        {
            _states = new IState[stateCount];
        }

        /// <summary>
        /// Adds the given <paramref name="state"/> to the <see cref="StateMachine"/>.
        /// The first state is automatically set as default State.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public IStateConfiguration Configure(IState state)
        {
            ValidateState(state);

            // set default set if first state
            if (_index == 0)
            {
                _currentState = state;
            }

            _states[_index] = state;

            _index++;

            return state;
        }

        /// <summary>
        /// Sets the <see cref="CurrentState"/> of this StateMachine to
        /// the appropriate <see cref="IState"/> based on the given <paramref name="transition"/>.
        /// </summary>
        /// <param name="transition"></param>
        public void PerformTransition(int transition)
        {
            var nextState = _currentState.GetStateFromTransition(transition);

            for (int i = 0; i < _states.Length; i++)
            {
                if (_states[i] == nextState)
                {
                    _currentState.OnExit();
                    _currentState = _states[i];
                    _currentState.OnEntry();
                    return;
                }
            }

            throw new Exception($"{nextState.Name} State hasn't been configured");
        }

        private void ValidateState(IState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException($"{nameof(state)}");
            }

            if (_index >= _states.Length)
            {
                throw new ArgumentOutOfRangeException($"All ({_states.Length}) states have been configured." +
                    $"If you want to add more states increase the stateCount in constructor.");
            }

            for (int i = 0; i < _states.Length; i++)
            {
                if (_states[i] == state)
                {
                    throw new Exception($"{state.Name} state has already been configured.");
                }
            }
        }
    }
}
