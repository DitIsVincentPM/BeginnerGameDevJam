using UnityEngine;

public class DefaultAbilityState : AbilityBaseState
{
    public DefaultAbilityState(AbilityStateMachine currentContext, AbilityStateFactory abilityStateFactory)
    : base(currentContext, abilityStateFactory) { }

    public override void EnterState() { }

    public override void ExitState() { }

    public override void UpdateState() { }
}
