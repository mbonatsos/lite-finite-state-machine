using System;

namespace FiniteStateMachine
{
    public interface IStateConfiguration
    {
        /// <summary>
        /// The action that will be executed when transitioning into this state.
        /// </summary>
        /// <param name="action">Action that will be executed.</param>
        /// <returns></returns>
        IStateConfiguration OnEntry(Action action);

        /// <summary>
        /// The action that will be executed when transitioning out of this state.
        /// </summary>
        /// <param name="action">Action that will be executed.</param>
        /// <returns></returns>
        IStateConfiguration OnExit(Action action);

        /// <summary>
        /// Links a transition to a <see cref="IState"/>.
        /// </summary>
        /// <param name="transition">Transition to link.</param>
        /// <param name="state">State to link.</param>
        /// <returns></returns>
        IStateConfiguration Link(int transition, IState state);
    }
}