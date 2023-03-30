using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform _target;
    private void Start() 
    {
        _target = GameObject.FindWithTag("Player").transform;
    }
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - _target.position);
    }
}
