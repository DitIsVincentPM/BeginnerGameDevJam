using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    public GameObject LaserTrigger;

    [SerializeField]
    Collider LaserCollider;

    [SerializeField]
    public Transform[] points;
    [SerializeField] bool isActive;

    // Start is called before the first frame update
    void Start() { 
        LaserTrigger = gameObject.transform.GetChild(0).gameObject;
        LaserCollider = gameObject.transform.GetChild(0).GetComponent<Collider>();
        points = gameObject.transform.GetChild(1).GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update() { 

    }
}
