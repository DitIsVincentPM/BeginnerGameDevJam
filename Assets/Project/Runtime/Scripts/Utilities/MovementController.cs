// Some stupid rigidbody based movement by Dani

using UnityEngine;

public class MovementController : MonoBehaviour
{
    public Animator animationController;
    Rigidbody rigidBody;

    //Assingables
    public Transform playerCam;
    public Transform orientation;
    BoxCollider feet;

    //Rotation and look
    float xRotation = 0;
    const float sensitivity = 50f;

    //Movement
    public float moveSpeed = 4500;
    public float maxSpeed = 20;
    bool grounded;

    const float threshold = 0.01f;
    public float maxSlopeAngle = 45f;

    //Crouch & Slide
    Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    Vector3 playerScale;
    public float slideForce = 400;

    public float friction = 0.175f;
    public float slideFriction = 0.2f;

    //Jumping
    public float jumpForce = 550f;

    //Input
    Vector2 inputDirection = new Vector2();
    bool crouching;

    //Sliding
    Vector3 normalVector = Vector3.up;

    void Awake()
    {
        feet = GetComponent<BoxCollider>();
        rigidBody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        playerScale = transform.localScale;
        LockCursor();
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        Movement();
    }

    void Update()
    {
        MyInput();
        Look();
    }

    /// <summary>
    /// Find user input. Should put this in its own class but im lazy
    /// </summary>
    void MyInput()
    {
        inputDirection.x = Input.GetAxisRaw("Horizontal");
        inputDirection.y = Input.GetAxisRaw("Vertical");
        inputDirection.Normalize();

        if (Input.GetButton("Jump"))
        {
            Jump();
        }

        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            animationController.SetBool("movingLeft", false);
            animationController.SetBool("movingRight", false);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            animationController.SetBool("movingLeft", true);
            animationController.SetBool("movingRight", false);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            animationController.SetBool("movingLeft", false);
            animationController.SetBool("movingRight", true);
        }
        else
        {
            animationController.SetBool("movingLeft", false);
            animationController.SetBool("movingRight", false);
        }

        //Crouching
        if (Input.GetKeyDown(KeyCode.LeftControl))
            StartCrouch();
        if (Input.GetKeyUp(KeyCode.LeftControl))
            StopCrouch();

        feet.enabled = inputDirection.magnitude == 0 && !crouching;
    }

    void StartCrouch()
    {
        crouching = true;
    }

    void StopCrouch()
    {
        crouching = false;
    }

    void Movement()
    {
        rigidBody.AddForce(Vector3.down * 10 * Time.deltaTime);

        ApplyFriction();

        //Set max speed
        float maxSpeed = this.maxSpeed;

        //Some multipliers
        float multiplier = 1f,
            multiplierV = 1f;

        // Movement in air
        if (!grounded)
        {
            multiplier = 0.5f;
            multiplierV = 0.5f;
        }

        //Apply forces to move player
        // more easily adjust left/right while in the air than forward/back
        rigidBody.AddForce(
            orientation.transform.right * inputDirection.x * moveSpeed * Time.deltaTime * multiplier
        );
        rigidBody.AddForce(
            orientation.transform.forward
                * inputDirection.y
                * moveSpeed
                * Time.deltaTime
                * multiplier
                * multiplierV
        );

        if (rigidBody.velocity.magnitude > maxSpeed)
        {
            rigidBody.velocity = rigidBody.velocity.normalized * maxSpeed;
        }

        if (rigidBody.velocity.magnitude < 2)
        {
            animationController.SetFloat("walkingSpeed", 0);
        }
        else
        {
            animationController.SetFloat("walkingSpeed", (rigidBody.velocity.magnitude / 5));
        }
    }

    void Jump()
    {
        if (grounded)
        {
            grounded = false;
            animationController.SetBool("isGrounded", false);
            rigidBody.AddForce(normalVector * jumpForce);
        }
    }

    void Look()
    {
        orientation.transform.eulerAngles = new Vector3(0, playerCam.transform.eulerAngles.y, 0);
    }

    void ApplyFriction()
    {
        if (!grounded)
            return;

        //Slow down sliding
        if (crouching)
        {
            rigidBody.AddForce(
                moveSpeed * Time.deltaTime * -rigidBody.velocity.normalized * slideFriction
            );
            return;
        }

        Vector3 inverseVelocity = -orientation.InverseTransformDirection(rigidBody.velocity);

        if (inputDirection.x == 0)
        {
            rigidBody.AddForce(
                inverseVelocity.x
                    * orientation.transform.right
                    * moveSpeed
                    * friction
                    * Time.deltaTime
            );
        }
        if (inputDirection.y == 0)
        {
            rigidBody.AddForce(
                inverseVelocity.z
                    * orientation.transform.forward
                    * moveSpeed
                    * friction
                    * Time.deltaTime
            );
        }
    }

    bool IsFloorAngle(Vector3 v)
    {
        float angle = Vector3.Angle(Vector3.up, v);
        return angle < maxSlopeAngle;
    }

    bool cancellingGrounded;

    /// <summary>
    /// Handle ground detection
    /// </summary>
    void OnCollisionStay(Collision other)
    {
        int layer = other.gameObject.layer;
        int ground = LayerMask.NameToLayer("Ground");
        if (layer != ground)
            return;

        //Iterate through every collision in a physics update
        for (int i = 0; i < other.contactCount; i++)
        {
            Vector3 normal = other.contacts[i].normal;
            if (IsFloorAngle(normal))
            {
                animationController.SetBool("isGrounded", true);
                grounded = true;
                normalVector = normal;
                cancellingGrounded = false;
                CancelInvoke(nameof(StopGrounded));
            }
        }

        //Invoke ground/wall cancel, since we can't check normals with CollisionExit
        if (!cancellingGrounded)
        {
            cancellingGrounded = true;
            Invoke(nameof(StopGrounded), Time.deltaTime);
        }
    }

    private void StopGrounded()
    {
        grounded = false;
    }
}
