using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastController : MonoBehaviour
{
    public RaycastHit GetRaycastHit(float maxDistance, LayerMask layer) {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, maxDistance, layer)) {
            return hit;
        }

        return default(RaycastHit);
    }
}
