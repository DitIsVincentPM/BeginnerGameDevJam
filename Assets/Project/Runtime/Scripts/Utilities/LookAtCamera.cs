using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform _target;
    private void Start() 
    {
        _target = GameObject.FindWithTag("MainCamera").transform;
    }
    void Update()
    {
        Vector3 relativePos = _target.position - transform.position;
        Quaternion LookAtRotation = Quaternion.LookRotation(relativePos);
        Quaternion LookAtRotationOnly_Y = Quaternion.Euler(transform.rotation.eulerAngles.x, LookAtRotation.eulerAngles.y - 180, transform.rotation.eulerAngles.z);
        transform.rotation = LookAtRotationOnly_Y;
    }
}
