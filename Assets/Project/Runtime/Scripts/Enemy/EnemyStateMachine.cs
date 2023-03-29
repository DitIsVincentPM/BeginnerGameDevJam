using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    private EnemyBaseState _currentState;
    private EnemyStateFactory _states;

    public EnemyBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }

    private void Awake()
    {
        _states = new EnemyStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();
    }
}
