using FluentAssertions;
using System;
using Xunit;

namespace FiniteStateMachine.Tests
{
    public class StateMachineTests
    {
        [Fact]
        public void GetCurrentState_WithValidTransitions_ReturnsAttackState()
        {
            // Arrange
            var idleState = new IdleState();
            var chaseState = new ChaseState();
            var attackState = new AttackState();
            var stateMachine = new StateMachine(3);

            stateMachine.Configure(idleState)
                .Link((int)Transition.EnemyInProximity, chaseState);

            stateMachine.Configure(chaseState)
                .Link((int)Transition.EnemyInAttackRange, attackState)
                .Link((int)Transition.EnemyOutOfRange, idleState);

            stateMachine.Configure(attackState)
                .Link((int)Transition.EnemyOutOfAttackRange, chaseState)
                .Link((int)Transition.EnemyOutOfRange, idleState);

            // Act
            stateMachine.PerformTransition((int)Transition.EnemyInProximity);
            stateMachine.PerformTransition((int)Transition.EnemyOutOfRange);
            stateMachine.PerformTransition((int)Transition.EnemyInProximity);
            stateMachine.PerformTransition((int)Transition.EnemyInAttackRange);

            // Assert
            stateMachine.CurrentState.Should().Be(attackState);
        }

        [Fact]
        public void Configure_WithAlreadyConfiguredState_ThrowsException()
        {
            // Arrange
            var idleState = new IdleState();
            var chaseState = new ChaseState();
            var attackState = new AttackState();
            var stateMachine = new StateMachine(3);

            // Act
            stateMachine.Configure(idleState)
                .Link((int)Transition.EnemyInProximity, chaseState);

            stateMachine.Configure(chaseState)
                .Link((int)Transition.EnemyInAttackRange, attackState)
                .Link((int)Transition.EnemyOutOfRange, idleState);

            Action action = () => stateMachine.Configure(chaseState);

            // Assert
            action.Should().Throw<Exception>();
        }

        [Fact]
        public void Configure_WithNullState_ThrowsArgumentNullException()
        {
            // Arrange
            var idleState = new IdleState();
            var chaseState = new ChaseState();
            var attackState = new AttackState();
            var stateMachine = new StateMachine(3);

            // Act
            Action action = () => stateMachine.Configure(null)
                .Link((int)Transition.EnemyInProximity, chaseState);

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Configure_WithMoreStates_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var idleState = new IdleState();
            var chaseState = new ChaseState();
            var attackState = new AttackState();
            var stateMachine = new StateMachine(2);

            stateMachine.Configure(idleState)
                .Link((int)Transition.EnemyInProximity, chaseState);

            stateMachine.Configure(chaseState)
                .Link((int)Transition.EnemyInAttackRange, attackState)
                .Link((int)Transition.EnemyOutOfRange, idleState);

            // Act
            Action action = () => stateMachine.Configure(attackState);

            // Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Link_WithNullState_ThrowsArgumentNullException()
        {
            // Arrange
            var idleState = new IdleState();
            var stateMachine = new StateMachine(2);

            // Act
            Action action = () => stateMachine.Configure(idleState)
                .Link((int)Transition.EnemyInProximity, null);

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Link_WithExistingTransitionInState_ThrowsException()
        {
            // Arrange
            var idleState = new IdleState();
            var chasteState = new ChaseState();
            var stateMachine = new StateMachine(2);

            // Act
            Action action = () => stateMachine.Configure(idleState)
                .Link((int)Transition.EnemyInProximity, chasteState)
                .Link((int)Transition.EnemyInProximity, chasteState);

            // Assert
            action.Should().Throw<Exception>();
        }

        [Fact]
        public void PerformTransition_WithInvalidTransition_ThrowsException()
        {
            // Arrange
            var idleState = new IdleState();
            var chaseState = new ChaseState();
            var stateMachine = new StateMachine(2);

            stateMachine.Configure(idleState)
                .Link((int)Transition.EnemyInProximity, chaseState);

            // Act
            Action action = () => stateMachine.PerformTransition((int)Transition.EnemyOutOfRange);

            // Assert
            action.Should().Throw<Exception>();
        }
    }

    internal enum Transition
    {
        EnemyInProximity = 0,
        EnemyOutOfRange = 1,
        EnemyInAttackRange = 2,
        EnemyOutOfAttackRange = 3,
    }

    internal class IdleState : State
    {
        public override string Name => nameof(IdleState);
    }

    internal class ChaseState : State
    {
        public override string Name => nameof(ChaseState);
    }

    internal class AttackState : State
    {
        public override string Name => nameof(AttackState);
    }
}
