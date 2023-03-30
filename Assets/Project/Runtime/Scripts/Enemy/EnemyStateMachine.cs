using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : MonoBehaviour
{
    private EnemyBaseState _currentState;
    private EnemyStateFactory _states;

    [SerializeField] private Transform _target;
    private NavMeshAgent _agent;
    [SerializeField] private LayerMask _whatIsGround, _whatIsTarget;

    private Vector3 _walkPoint;
    private bool _walkPointSet;
    [SerializeField] private float _walkPointRange;

    [SerializeField] private float _attackCooldown;
    private bool _alreadyAttacked;

    [SerializeField] private float _sightRange, _attackRange;
    private bool _targetInSightRange, _targetInAttackRange;

    public Transform Target { get { return _target; } }
    public NavMeshAgent Agent { get { return _agent; } }
    public LayerMask WhatIsGround { get { return _whatIsGround; } }

    public Vector3 WalkPoint { get { return _walkPoint; } set { _walkPoint = value; } }
    public bool WalkPointSet { get { return _walkPointSet; } set { _walkPointSet = value; } }
    public float WalkPointRange { get { return _walkPointRange; } }

    public bool TargetInSightRange { get { return _targetInSightRange; } }
    public bool TargetInAttackRange { get { return _targetInAttackRange; } }

    public EnemyBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        _states = new EnemyStateFactory(this);
        _currentState = _states.Patrolling();
        _currentState.EnterState();
    }

    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        _targetInSightRange = Physics.CheckSphere(transform.position, _sightRange, _whatIsTarget);
        _targetInAttackRange = Physics.CheckSphere(transform.position, _attackRange, _whatIsTarget);
        _currentState.UpdateState();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _sightRange);
    }
}
