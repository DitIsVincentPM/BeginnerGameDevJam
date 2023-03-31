using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    private float _minHealth = 0.0f;
    [SerializeField] private float _health;

    private void Awake()
    {
        _health = _maxHealth;
    }

    public float Health { get { return _health; } }

    public void AddHealth(float amount)
    {
        float desiredAmount = _health + amount;
        _health = Mathf.Clamp(desiredAmount, _minHealth, _maxHealth);
    }
}
