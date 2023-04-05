using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserHitHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerFix"))
        {
            other.transform.parent.parent.GetComponent<PlayerController>().DrainBattery(20f);
        }
    }
}
