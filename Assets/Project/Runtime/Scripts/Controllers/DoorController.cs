using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public enum DoorActivation
    {
        Proximity,
        Function,
        StayOpen,
        StayClosed,
        None
    }

    [Header("Settings")]
    [SerializeField]
    DoorActivation activationState;

    [SerializeField]
    Animator animator;

    [SerializeField]
    GameObject player;

    [Header("Proximity")]
    [SerializeField]
    float proximityDistance = 5f;

    // Update is called once per frame
    void Update()
    {
        switch (activationState)
        {
            case DoorActivation.Proximity:
                if (
                    Vector3.Distance(player.transform.position, transform.position)
                    <= proximityDistance
                )
                {
                    animator.SetBool("doorOpen", true);
                }
                else
                    animator.SetBool("doorOpen", false);
                break;
            case DoorActivation.None:
                break;
            case DoorActivation.StayClosed:
                animator.SetBool("doorOpen", false);
                break;
            case DoorActivation.StayOpen:
                animator.SetBool("doorOpen", true);
                break;
        }
    }

    public void OpenDoor()
    {
        if (activationState == DoorActivation.Function)
        {
            animator.SetBool("doorOpen", true);
        }
    }

    public void CloseDoor()
    {
        if (activationState == DoorActivation.Function)
        {
            animator.SetBool("doorOpen", false);
        }
    }
}
