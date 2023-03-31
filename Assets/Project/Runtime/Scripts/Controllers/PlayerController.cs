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

    public float GetBattery()
    {
        return _playerEntity.Health;
    }

    public void DrainBattery(float drainValue)
    {
        _playerEntity.AddHealth(-drainValue);
        UISystem.singleton.BatterySlider.value = GetBattery();
        UISystem.singleton.BatteryPrecentage.text = Mathf.RoundToInt(GetBattery()).ToString() + "%";
    }
}
