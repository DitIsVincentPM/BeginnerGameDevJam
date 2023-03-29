using UnityEngine;

public class EnemyStateFactory
{
    private EnemyStateMachine _context;

    public EnemyStateFactory(EnemyStateMachine currentContext)
    {
        _context = currentContext;
    }

    public EnemyBaseState Idle()
    {
        return new EnemyIdleState(_context, this);
    }

    public EnemyBaseState Walk()
    {
        return new EnemyWalkState(_context, this);
    }

    public EnemyBaseState Grounded()
    {
        return new EnemyGroundedState(_context, this);
    }
}
