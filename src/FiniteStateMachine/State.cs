using System;
using System.Collections.Generic;

namespace FiniteStateMachine
{
    public abstract class State : IState
    {
        public abstract string Name { get; }

        private Dictionary<int, IState> _maps = new Dictionary<int, IState>();
        private Action _onEntry;
        private Action _onExit;

        /// <summary>
        /// Links a transition to a <see cref="IState"/>.
        /// </summary>
        /// <param name="transition"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        /// <exception cref="Exception">If the given transition is already linked.</exception>
        public IStateConfiguration Link(int transition, IState state)
        {
            Validate(transition, state);

            _maps.Add(transition, state);

            return this;
        }

        /// <summary>
        /// Configures the action that will be executed when transitioning into this state.
        /// </summary>
        /// <param name="action">Action that will be executed.</param>
        /// <returns></returns>
        public IStateConfiguration OnEntry(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            _onEntry = action;

            return this;
        }

        /// <summary>
        /// Configures the action that will be executed when transitioning out of this state.
        /// </summary>
        /// <param name="action">Action that will be executed.</param>
        /// <returns></returns>
        public IStateConfiguration OnExit(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            _onExit = action;

            return this;
        }

        /// <summary>
        /// Invokes the preconfigured <see cref="_onEntry"/> action.
        /// </summary>
        public void OnEntry()
        {
            _onEntry?.Invoke();
        }

        /// <summary>
        /// Invokes the preconfigured <see cref="_onExit"/> action.
        /// </summary>
        public void OnExit()
        {
            _onExit?.Invoke();
        }

        /// <summary>
        /// Returns the appropriate <see cref="IState"/> that is linked to the given <paramref name="transition"/>.
        /// </summary>
        /// <param name="transition"></param>
        /// <returns>The appropriate state.</returns>
        /// <exception cref="Exception">If the given transition is not linked to any state.</exception>
        public IState GetStateFromTransition(int transition)
        {
            if (!_maps.TryGetValue(transition, out var state))
            {
                throw new Exception($"There is no {transition} Transition in {Name} State.");
            }

            return state;
        }

        private void Validate(int transition, IState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state), $"Please set a valid {nameof(State)}.");
            }

            if (_maps.ContainsKey(transition))
            {
                throw new Exception($"Transition {transition} already exists in {Name} State.");
            }
        }
    }
}