using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserHitHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.transform.parent.gameObject.transform.parent
                .GetComponent<Entity>()
                .AddHealth(-5f);
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Entity>().AddHealth(-5f);
        }
    }
}
