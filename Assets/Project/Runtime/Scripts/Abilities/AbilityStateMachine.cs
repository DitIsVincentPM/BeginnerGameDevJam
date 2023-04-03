using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityStateMachine : MonoBehaviour
{
    private AbilityBaseState _currentState;
    private AbilityStateFactory _states;

    private List<AbilityBaseState> _abilities = new List<AbilityBaseState>();
    private int _currentAbility;
    private int _maxAbility;

    public AbilityBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }

    #region Object Pickup Ability
    [Header("Object Pickup Ability")]
    [SerializeField] private GameObject _player;
    [SerializeField] private Transform _camera;
    [SerializeField] private Transform _holdPos;
    [SerializeField] private string _pickupLayer;
    [SerializeField] private string _playerLayer;

    [SerializeField] private float _throwForce = 500f;
    [SerializeField] private float _pickUpRange = 10f;
    [SerializeField, Range(1f, 10f)] private float _rotationSensitivity = 1f;

    [SerializeField] InputActionReference _interact, _alpha1, _alpha2, _fire, _rotate;

    public GameObject Player { get { return _player; } }
    public Transform Camera { get { return _camera; } }
    public Transform HoldPos { get { return _holdPos; } }
    public string PickupLayer { get { return _pickupLayer; } }
    public string PlayerLayer { get { return _playerLayer; } }

    public float ThrowForce { get { return _throwForce; } }
    public float PickUpRange { get { return _pickUpRange; } }
    public float RotationSensitivity { get { return _rotationSensitivity; } }
    public InputActionReference Interact { get { return _interact; } }
    public InputActionReference Fire { get { return _fire; } }
    public InputActionReference Rotate { get { return _rotate; } }
    #endregion


    private void Awake()
    {
        _states = new AbilityStateFactory(this);

        _abilities.Add(_states.Default());
        _abilities.Add(_states.ObjectPickup());
        _maxAbility = _abilities.Count - 1;
        _currentAbility = 0;

        _currentState = _abilities[_currentAbility];
        _currentState.EnterState();
    }

    private void Update()
    {
        SwitchAbilitySystem();
        _currentState.UpdateState();
    }

    public void UnlockAbility(AbilityBaseState ability)
    {
        if (!_abilities.Contains(ability))
        {
            _abilities.Add(ability);
        }
    }

    private void SwitchAbilitySystem()
    {
        if (_abilities.Count != 0)
        {
            if (_alpha1.action.triggered)
            {
                if (_currentAbility != 0)
                {
                    _currentAbility--;
                }
                else
                {
                    _currentAbility = _maxAbility;
                }

                _currentState.SwitchState(_abilities[_currentAbility]);
                Debug.Log(_currentState);
            }
            else if (_alpha2.action.triggered)
            {
                if (_currentAbility < _maxAbility)
                {
                    _currentAbility++;
                }
                else if (_currentAbility == _maxAbility)
                {
                    _currentAbility = 0;
                }

                _currentState.SwitchState(_abilities[_currentAbility]);
                Debug.Log(_currentState);
            }
        }
    }
}
