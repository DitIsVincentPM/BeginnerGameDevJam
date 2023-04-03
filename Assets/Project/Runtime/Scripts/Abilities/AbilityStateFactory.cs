using UnityEngine;

public class AbilityStateFactory
{
    private AbilityStateMachine _context;

    public AbilityStateFactory(AbilityStateMachine currentContext)
    {
        _context = currentContext;
    }

    public AbilityBaseState ObjectPickup()
    {
        return new ObjectPickupAbilityState(_context, this);
    }
}
