using UnityEngine;

public class ObjectPickupAbilityState : AbilityBaseState
{
    public GameObject heldObj;
    private Rigidbody heldObjRb;
    private bool canDrop = true;
    private int LayerNumber;

    public ObjectPickupAbilityState(AbilityStateMachine currentContext, AbilityStateFactory abilityStateFactory)
    : base(currentContext, abilityStateFactory) { }

    public override void EnterState()
    {
        LayerNumber = LayerMask.NameToLayer("holdLayer");
    }

    public override void ExitState() { }

    public override void UpdateState()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObj == null)
            {
                RaycastHit hit;
                int layerMask = ~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer(Ctx.PlayerLayer));
                if (Physics.Raycast(Ctx.Camera.position, Ctx.Camera.TransformDirection(Vector3.forward), out hit, Ctx.PickUpRange, layerMask))
                {
                    if (hit.transform.gameObject.CompareTag("canPickUp"))
                    {
                        PickUpObject(hit.transform.gameObject);
                    }
                }
            }
            else
            {
                if (canDrop == true)
                {
                    StopClipping();
                    DropObject();
                }
            }
        }
        if (heldObj != null)
        {
            MoveObject();
            RotateObject();
            if (Input.GetKeyDown(KeyCode.Mouse0) && canDrop == true)
            {
                StopClipping();
                ThrowObject();
            }
        }
    }

    void PickUpObject(GameObject pickUpObj)
    {
        if (pickUpObj.GetComponent<Rigidbody>())
        {
            heldObj = pickUpObj;
            heldObjRb = pickUpObj.GetComponent<Rigidbody>();
            heldObjRb.isKinematic = true;
            heldObjRb.transform.parent = Ctx.HoldPos.transform;
            heldObj.layer = LayerNumber;
            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), Ctx.Player.GetComponent<Collider>(), true);
        }
    }

    void DropObject()
    {
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), Ctx.Player.GetComponent<Collider>(), false);

        heldObj.layer = LayerMask.NameToLayer(Ctx.PickupLayer);
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null;
        heldObj = null;
    }

    void MoveObject()
    {
        heldObj.transform.position = Ctx.HoldPos.transform.position;
    }

    void RotateObject()
    {
        if (Input.GetKey(KeyCode.R))
        {
            canDrop = false;

            float XaxisRotation = Input.GetAxis("Mouse X") * Ctx.RotationSensitivity;
            float YaxisRotation = Input.GetAxis("Mouse Y") * Ctx.RotationSensitivity;

            if (heldObj != null)
            {
                heldObj.transform.RotateAround(Ctx.HoldPos.position, Ctx.Player.transform.right, -YaxisRotation);
                heldObj.transform.RotateAround(Ctx.HoldPos.position, Ctx.Player.transform.up, XaxisRotation);
            }
        }
        else
        {
            canDrop = true;
        }
    }

    void ThrowObject()
    {
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), Ctx.Player.GetComponent<Collider>(), false);
        heldObj.layer = LayerMask.NameToLayer(Ctx.PickupLayer);
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null;
        heldObj.transform.position = Ctx.Player.transform.position + new Vector3(0f, 3f, 0f); // Set position to player position
        heldObjRb.AddForce(Ctx.Camera.forward * Ctx.ThrowForce);
        heldObj = null;
    }

    void StopClipping()
    {
        float minClipDistance = 0.2f; // adjust this value as needed

        var clipRange = Vector3.Distance(heldObj.transform.position, Ctx.Camera.position);

        RaycastHit[] hits;
        hits = Physics.RaycastAll(Ctx.Camera.position, Ctx.Camera.TransformDirection(Vector3.forward), clipRange);

        if (hits.Length > 1)
        {
            Vector3 desiredPos = Ctx.Player.transform.position + new Vector3(0f, 1f, 0f);
            float distanceToDesiredPos = Vector3.Distance(heldObj.transform.position, desiredPos);

            if (distanceToDesiredPos > minClipDistance)
            {
                heldObj.transform.position = desiredPos;
            }
        }
    }
}
