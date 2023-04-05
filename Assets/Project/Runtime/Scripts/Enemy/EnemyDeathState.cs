using System.Collections;
using UnityEditor;
using UnityEngine;

public class EnemyDeathState : EnemyBaseState
{
    private IEnumerator deathCouritine;

    public EnemyDeathState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory)
        : base(currentContext, enemyStateFactory)
    {
        IsRootState = true;
    }

    public override void CheckSwitchStates() { }

    public override void EnterState()
    {
        deathCouritine = InitializeDeath(2.0f);
        Ctx.StartCoroutine(deathCouritine);
    }

    private IEnumerator InitializeDeath(float waitTime)
    {
        Debug.Log("Death animation");
        yield return new WaitForSeconds(waitTime);
        GameObject.Destroy(Ctx.gameObject);
    }

    public override void ExitState() { }

    public override void InitializeSubState() { }

    public override void UpdateState() { }
}