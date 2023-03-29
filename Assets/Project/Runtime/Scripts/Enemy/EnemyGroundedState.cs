using UnityEngine;

public class EnemyGroundedState : EnemyBaseState
{
    public EnemyGroundedState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory)
    : base(currentContext, enemyStateFactory) { }

    public override void CheckSwitchStates() { }

    public override void EnterState() { }

    public override void ExitState() { }

    public override void InitializeSubState() { }

    public override void UpdateState() { }
}
