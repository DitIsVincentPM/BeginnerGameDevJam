using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISystem : StaticInstance<UISystem>
{
    [Header("UI Elements")]
    public TMP_Text BatteryPrecentage;
    public Slider BatterySlider;
    public Slider NotificationSlider;
}

