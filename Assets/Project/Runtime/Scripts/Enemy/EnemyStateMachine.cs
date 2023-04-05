using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : MonoBehaviour
{
    private EnemyBaseState _currentState;
    private EnemyStateFactory _states;

    [SerializeField]
    private LayerMask _whatIsGround,
        _whatIsTarget;

    [SerializeField]
    private Transform _target;
    private Entity _enemyEntity;
    private NavMeshAgent _agent;

    private Vector3 _walkPoint;
    private bool _walkPointSet;

    [SerializeField]
    private float _walkPointRange;

    [SerializeField]
    private float _attackCooldown;

    [SerializeField]
    private float _attackDamage;
    private bool _alreadyAttacked;
    private AttackHandler _attackHandler;

    [SerializeField]
    private float _sightRange,
        _attackRange;
    private bool _targetInSightRange,
        _targetInSphereRange,
        _targetInAttackRange;

    [SerializeField]
    public Animator animator;

    private bool _isDead = false;

    public Transform Target
    {
        get { return _target; }
    }
    public NavMeshAgent Agent
    {
        get { return _agent; }
    }
    public LayerMask WhatIsGround
    {
        get { return _whatIsGround; }
    }

    public Vector3 WalkPoint
    {
        get { return _walkPoint; }
        set { _walkPoint = value; }
    }
    public bool WalkPointSet
    {
        get { return _walkPointSet; }
        set { _walkPointSet = value; }
    }
    public float WalkPointRange
    {
        get { return _walkPointRange; }
    }

    public bool TargetInSightRange
    {
        get { return _targetInSightRange; }
    }
    public bool TargetInAttackRange
    {
        get { return _targetInAttackRange; }
    }

    public float AttackCooldown
    {
        get { return _attackCooldown; }
    }
    public float AttackDamage
    {
        get { return _attackDamage; }
    }
    public bool AlreadyAttacked
    {
        get { return _alreadyAttacked; }
        set { _alreadyAttacked = value; }
    }

    public EnemyBaseState CurrentState
    {
        get { return _currentState; }
        set { _currentState = value; }
    }

    public bool IsDead
    {
        get { return _isDead; }
    }

    private void Awake()
    {
        animator = transform.GetChild(1).GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _enemyEntity = GetComponent<Entity>();

        _states = new EnemyStateFactory(this);
        _currentState = _states.Patrolling();
        _currentState.EnterState();
    }

    private void OnEnable()
    {
        _attackHandler = GetComponentInChildren<AttackHandler>();
        _attackHandler.OnAttack += Attack;
        _enemyEntity.OnDeath += OnDeath;
    }

    private void OnDisable()
    {
        _attackHandler.OnAttack -= Attack;
        _enemyEntity.OnDeath -= OnDeath;
    }

    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (_currentState.ToString() == "EnemyPatrollingState" && _agent.velocity.magnitude < 0.1f)
        {
            animator.SetBool("walking", false);
        }
        else
        {
            animator.SetBool("walking", true);
        }

        _targetInSphereRange = Physics.CheckSphere(transform.position, _sightRange, _whatIsTarget);

        if (_targetInSphereRange)
        {
            // Check if player is visible (i.e., not obstructed by a wall)
            RaycastHit hit;
            Ray raycast = new Ray(
                transform.position + new Vector3(0, 2, 0),
                _target.position - transform.position
            );
            Debug.DrawRay(raycast.origin, raycast.direction * _sightRange, new Color(1, 0, 0));

            if (Physics.Raycast(raycast, out hit, _sightRange))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    _targetInSightRange = true;
                }
                else
                {
                    _targetInSightRange = false;
                }
            }
            else
            {
                _targetInSightRange = false;
            }
        }
        else
        {
            _targetInSightRange = false;
        }

        _targetInAttackRange = Physics.CheckSphere(transform.position, _attackRange, _whatIsTarget);
        _currentState.UpdateState();
    }

    public void Attack()
    {
        if (_targetInAttackRange)
        {
            SoundSystem.Instance.PlaySound(SoundSystem.Instance.Punch, transform.position);
            _target.GetComponent<PlayerController>().DrainBattery(_attackDamage);
        }
        _alreadyAttacked = true;
        Invoke(nameof(ResetAttack), _attackCooldown);
    }

    public void OnDeath()
    {
        _isDead = true;
    }

    public void ResetAttack()
    {
        _alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _sightRange);
    }
}
