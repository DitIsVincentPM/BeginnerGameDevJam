using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserHitHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.transform.parent.GetComponentInParent<Entity>().AddHealth(-5f);
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.transform.parent.GetComponentInParent<Entity>().AddHealth(-15f);
        }
    }
}
