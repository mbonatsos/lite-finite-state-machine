namespace FiniteStateMachine
{
    public interface IState : IStateConfiguration
    {
        /// <summary>
        /// The name of the state.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Returns the appropriate <see cref="IState"/> that is linked to the given <paramref name="transition"/>.
        /// </summary>
        /// <param name="transition"></param>
        /// <returns>The preconfigured state.</returns>
        IState GetStateFromTransition(int transition);

        /// <summary>
        /// Invokes the preconfigured action.
        /// </summary>
        void OnEntry();

        /// <summary>
        /// Invokes the preconfigured action.
        /// </summary>
        void OnExit();
    }
}