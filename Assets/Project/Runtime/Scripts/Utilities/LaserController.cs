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

    [SerializeField]
    public bool isActive;
    int currentPoint;
    int rndPoint;

    // Start is called before the first frame update
    void Start()
    {
        LaserTrigger = gameObject.transform.GetChild(0).gameObject;
        LaserCollider = gameObject.transform.GetChild(0).GetComponent<Collider>();

        rndPoint = Random.Range(1,3)-1;
        currentPoint = rndPoint;
        LaserTrigger.transform.position = points[rndPoint].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if(!LaserTrigger.activeSelf) LaserTrigger.SetActive(true);
            if (
                Vector3.Distance(LaserTrigger.transform.position, points[1].position) > 0.001f
                && currentPoint == 0
            )
            {
                LaserTrigger.transform.position = Vector3.MoveTowards(
                    LaserTrigger.transform.position,
                    points[1].position,
                    0.02f
                );
                if (Vector3.Distance(LaserTrigger.transform.position, points[1].position) < 0.001f)
                {
                    currentPoint = 1;
                }
            }
            else if (
                Vector3.Distance(LaserTrigger.transform.position, points[0].position) > 0.001f
                && currentPoint == 1
            )
            {
                LaserTrigger.transform.position = Vector3.MoveTowards(
                    LaserTrigger.transform.position,
                    points[0].position,
                    0.02f
                );
                if (Vector3.Distance(LaserTrigger.transform.position, points[0].position) < 0.001f)
                {
                    currentPoint = 0;
                }
            }
        } else {
            if(LaserTrigger.activeSelf) LaserTrigger.SetActive(false);
        }
    }
}
