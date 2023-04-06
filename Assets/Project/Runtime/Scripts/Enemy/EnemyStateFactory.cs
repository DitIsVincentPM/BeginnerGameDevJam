using UnityEngine;

public class EnemyStateFactory
{
    private EnemyStateMachine _context;

    public EnemyStateFactory(EnemyStateMachine currentContext)
    {
        _context = currentContext;
    }

    public EnemyBaseState Patrolling()
    {
        return new EnemyPatrollingState(_context, this);
    }

    public EnemyBaseState Chase()
    {
        return new EnemyChaseState(_context, this);
    }

    public EnemyBaseState Attack()
    {
        return new EnemyAttackState(_context, this);
    }

    public EnemyBaseState Death()
    {
        return new EnemyDeathState(_context, this);
    }
}
