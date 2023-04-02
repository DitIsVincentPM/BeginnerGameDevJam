public abstract class AbilityBaseState
{
    private AbilityStateMachine _ctx;
    private AbilityStateFactory _factory;

    protected AbilityStateMachine Ctx { get { return _ctx; } }
    protected AbilityStateFactory Factory { get { return _factory; } }

    public AbilityBaseState(AbilityStateMachine currentContext, AbilityStateFactory abilityStateFactory)
    {
        _ctx = currentContext;
        _factory = abilityStateFactory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();

    public void SwitchState(AbilityBaseState newState)
    {
        ExitState();
        newState.EnterState();

        _ctx.CurrentState = newState;
    }
}
