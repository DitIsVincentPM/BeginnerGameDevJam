using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    public EnemyChaseState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory)
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
        if (Ctx.TargetInSightRange && Ctx.TargetInAttackRange)
        {
            SwitchState(Factory.Attack());
        }
    }

    public override void EnterState()
    {
        Ctx.animator.SetBool("walking", true);
    }

    public override void ExitState()
    {
        Ctx.animator.SetBool("walking", false);
    }

    public override void InitializeSubState() { }

    public override void UpdateState()
    {
        CheckSwitchStates();
        Ctx.Agent.SetDestination(Ctx.Target.position);
    }
}
