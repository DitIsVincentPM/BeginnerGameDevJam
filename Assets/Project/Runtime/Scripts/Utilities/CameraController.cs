using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform player;

    void Update() {
        transform.position = player.transform.position;
    }
}