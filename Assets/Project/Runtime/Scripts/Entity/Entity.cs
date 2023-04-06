using System;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _health;
    private float _minHealth = 0.0f;
    private bool _isAlive = true;

    public event Action OnDeath;

    public float Health { get { return _health; } }

    private void Awake()
    {
        _health = _maxHealth;
    }

    private void Update()
    {
        if (Health <= _minHealth && _isAlive)
        {
            _isAlive = false;
            Death();
        }
    }

    public void AddHealth(float amount)
    {
        float desiredAmount = _health + amount;
        _health = Mathf.Clamp(desiredAmount, _minHealth, _maxHealth);
    }

    public void Death()
    {
        OnDeath?.Invoke();
    }
}
