using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateLaser : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Transform[] Lasers;

    public void DeactiveLasers() { 
        foreach(Transform laser in Lasers) {
            laser.gameObject.GetComponentInParent<LaserController>().isActive = false;
        }
    }
}
