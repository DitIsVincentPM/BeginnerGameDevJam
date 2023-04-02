using UnityEngine;
public class CameraMainMenu : MonoBehaviour
{
    public Transform target; // the target object to rotate around
    public float distance = 10.0f; // the distance from the target object
    public float speed = 1.0f; // the speed of camera movement around the target object
    public Vector3 offset = Vector3.zero; // the offset from the target object

    float angle = 0.0f;

    void Start()
    {
        if (!target)
        {
            Debug.LogError("Target object not assigned!");
        }
    }

    void Update()
    {
        // Calculate the position based on angle and distance
        Vector3 position = target.position + offset;
        position.x += distance * Mathf.Sin(angle * Mathf.Deg2Rad);
        position.z += distance * Mathf.Cos(angle * Mathf.Deg2Rad);

        // Update the camera position and rotation
        transform.position = position;
        transform.LookAt(target.position);

        // Increase the angle for the next frame
        angle += speed * Time.deltaTime;
        if (angle >= 360.0f)
        {
            angle = 0.0f;
        }
    }

    public void StartGame() {
        GetComponent<CameraController>().enabled = true;
        GetComponent<CameraMainMenu>().enabled = false;
    }
}
