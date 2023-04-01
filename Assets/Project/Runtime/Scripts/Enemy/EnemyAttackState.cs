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
       // Enter state
    }

    public override void ExitState() { }

    public override void InitializeSubState() { }

    public override void UpdateState()
    {
        CheckSwitchStates();
        Ctx.Agent.SetDestination(Ctx.transform.position);

        if (!Ctx.AlreadyAttacked)
        {
            Ctx.Target.GetComponent<PlayerController>().DrainBattery(Ctx.AttackDamage);

            Ctx.AlreadyAttacked = true;
            Ctx.Invoke(nameof(Ctx.ResetAttack), Ctx.AttackCooldown);
        }
    }
}
