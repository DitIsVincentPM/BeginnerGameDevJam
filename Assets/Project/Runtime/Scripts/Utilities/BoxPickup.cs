using UnityEngine;

public class BoxPickup : MonoBehaviour
{
    [SerializeField] private float pickupDistance = 2f;
    [SerializeField] private LayerMask boxLayerMask;
    [SerializeField] private GameObject worldCanvas;
    [SerializeField] private float distanceFromPlayer;
    private Camera mainCamera;
    private Rigidbody heldBox;
    private bool isHoldingBox;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Check if there's a box in front of the player that can be picked up
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, pickupDistance, boxLayerMask))
        {
            Rigidbody boxRigidbody = hit.collider.GetComponent<Rigidbody>();
            if (boxRigidbody != null && (!boxRigidbody.isKinematic || isHoldingBox))
            {
                // Enable the world canvas
                worldCanvas.SetActive(true);

                // Pick up or drop the box
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (heldBox == null)
                    {
                        // Pick up the box
                        heldBox = boxRigidbody;
                        heldBox.isKinematic = true;
                        heldBox.transform.parent = mainCamera.transform;
                        heldBox.transform.localPosition = Vector3.forward * distanceFromPlayer;
                        isHoldingBox = true;
                    }
                    else
                    {
                        // Drop the box
                        heldBox.isKinematic = false;
                        heldBox.transform.parent = null;
                        heldBox = null;
                        isHoldingBox = false;
                    }
                }
            }
            else
            {
                // Disable the world canvas if the box is not pickable
                worldCanvas.SetActive(false);
            }
        }
        else
        {
            // Disable the world canvas if the player is not looking at the box
            worldCanvas.SetActive(false);
        }
    }
}
