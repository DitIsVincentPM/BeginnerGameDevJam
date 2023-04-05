using UnityEngine;

public class EnemyDeathState : EnemyBaseState
{
    public EnemyDeathState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory)
        : base(currentContext, enemyStateFactory)
    {
        IsRootState = true;
    }

    public override void CheckSwitchStates() { }

    public override void EnterState()
    {
        Debug.Log("Dead");
    }

    public override void ExitState() { }

    public override void InitializeSubState() { }

    public override void UpdateState() { }
}