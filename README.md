# FiniteStateMachine

#### How to use
```csharp
var idleState = new IdleState();
var chaseState = new ChaseState();
var stateMachine = new StateMachine(stateCount: 2);

stateMachine.Configure(idleState)
    .Link((int)Transition.EnemyInProximity, chaseState);

stateMachine.Configure(chaseState)
    .Link((int)Transition.EnemyOutOfRange, idleState);

stateMachine.PerformTransition((int)Transition.EnemyInProximity);

// ChaseState
Console.WriteLine(stateMachine.CurrentState);
```
