using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public EnemyAttackState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory)
    : base(currentContext, enemyStateFactory)
    {
        IsRootState = true;
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.TargetInSightRange && !Ctx.TargetInAttackRange)
        {
            SwitchState(Factory.Patrolling());
        }
        if (Ctx.TargetInSightRange && !Ctx.TargetInAttackRange)
        {
            SwitchState(Factory.Chase());
        }
    }

    public override void EnterState()
    {
        Debug.Log("Attacking");
    }

    public override void ExitState() { }

    public override void InitializeSubState() { }

    public override void UpdateState()
    {
        CheckSwitchStates();
        Ctx.Agent.SetDestination(Ctx.transform.position);
        Ctx.transform.LookAt(Ctx.Target);
    }
}
