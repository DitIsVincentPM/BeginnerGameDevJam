using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public DoorController.DoorActivation DoorState;
    public DoorController door;

    private void OnTriggerEnter(Collider other)
    {
        door.activationState = DoorState;
    }
}

