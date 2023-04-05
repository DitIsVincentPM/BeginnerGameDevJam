using UnityEngine;

public class EnemyPatrollingState : EnemyBaseState
{
    public EnemyPatrollingState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory)
    : base(currentContext, enemyStateFactory)
    {
        IsRootState = true;
    }

    public override void CheckSwitchStates()
    {
        if (Ctx.IsDead)
        {
            SwitchState(Factory.Death());
        }
        if (Ctx.TargetInSightRange && !Ctx.TargetInAttackRange)
        {
            SwitchState(Factory.Chase());
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

        if (!Ctx.WalkPointSet)
        {
            SearchWalkPoint();
        }
        else
        {
            Ctx.Agent.SetDestination(Ctx.WalkPoint);
        }

        Vector3 distanceToWalkPoint = Ctx.transform.position - Ctx.WalkPoint;

        if (distanceToWalkPoint.magnitude < 1.0f)
        {
            Ctx.WalkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randomX = Random.Range(-Ctx.WalkPointRange, Ctx.WalkPointRange);
        float randomZ = Random.Range(-Ctx.WalkPointRange, Ctx.WalkPointRange);

        Ctx.WalkPoint = new Vector3(
            Ctx.transform.position.x + randomX,
            Ctx.transform.position.y,
            Ctx.transform.position.z + randomZ
        );

        if (Physics.Raycast(Ctx.WalkPoint, -Ctx.transform.up, 2.0f, Ctx.WhatIsGround))
        {
            Ctx.WalkPointSet = true;
        }
    }
}
