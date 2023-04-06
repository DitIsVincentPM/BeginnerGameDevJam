using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    public string partToDisable;

    private void OnTriggerEnter(Collider other)
    {
        GameplayHandler.Instance.CloseMapPart(partToDisable);
    }
}
