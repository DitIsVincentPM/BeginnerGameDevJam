using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Value's")]
    public List<string> inInventory;
    private Entity _playerEntity;

    private void Awake()
    {
        _playerEntity = GetComponent<Entity>();
    }

    private void OnEnable()
    {
        _playerEntity.OnDeath += OnDeath;
    }

    private void OnDisable()
    {
        _playerEntity.OnDeath -= OnDeath;
    }

    private void OnDeath()
    {

    }

    public float GetBattery()
    {
        return _playerEntity.Health;
    }

    public void DrainBattery(float drainValue)
    {
        _playerEntity.AddHealth(-drainValue);
        UISystem.Instance.BatterySlider.value = GetBattery();
        UISystem.Instance.BatteryPrecentage.text = Mathf.RoundToInt(GetBattery()).ToString() + "%";
    }
}
