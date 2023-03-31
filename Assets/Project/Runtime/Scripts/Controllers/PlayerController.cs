using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Value's")]
    private float Battery = 100;

    public float GetBattery() {
        return Battery;
    }

    public bool DrainBattery(float drainValue) {
        if(Battery > drainValue) {
            Battery -= drainValue;

            UISystem.singleton.BatterySlider.value = Battery;
            UISystem.singleton.BatteryPrecentage.text = Mathf.RoundToInt(Battery).ToString() + "%";
            return true;
        }
        return false;
    }
}
