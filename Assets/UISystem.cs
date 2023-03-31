using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISystem : MonoBehaviour
{
    public static new UISystem singleton { get; set; }

    [Header("UI Elements")]
    public TMP_Text BatteryPrecentage;
    public Slider BatterySlider;

    void Awake() {
        singleton = this;
    }
}

