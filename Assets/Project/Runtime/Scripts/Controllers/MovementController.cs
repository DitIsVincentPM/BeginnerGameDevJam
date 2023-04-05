// Some stupid rigidbody based movement by Dani

using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    public CameraEffects camEffects;
    public Animator animationController;
    public GameObject groundParticle;

    Rigidbody rigidBody;

    //Assingables
    public Transform playerCam;
    public Transform orientation;
    CapsuleCollider feet;

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
    bool canAttack;
    public float attackCooldown = 2;
    float attackCooldownTimer;
    public float attackDamage = -10f;
    public int attackRange = 2;

    [SerializeField]
    private InputActionReference move, jump, fire;

    private void OnEnable()
    {
        jump.action.performed += JumpPerformed;
    }

    private void OnDisable()
    {
        jump.action.performed -= JumpPerformed;
    }

    private void JumpPerformed(InputAction.CallbackContext obj)
    {
        Jump();
    }

    void Awake()
    {
        feet = transform.GetChild(0).GetChild(0).GetComponent<CapsuleCollider>();
        rigidBody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        playerScale = transform.localScale;
    }

    void FixedUpdate()
    {
        if (GameplayHandler.Instance.currentPuzzle < 0)
            return;
        Movement();
    }

    void Update()
    {
        if (GameplayHandler.Instance.currentPuzzle < 0)
            return;
        MyInput();
        Look();
    }

    /// <summary>
    /// Find user input. Should put this in its own class but im lazy
    /// </summary>
    void MyInput()
    {
        inputDirection.x = move.action.ReadValue<Vector2>().x;
        inputDirection.y = move.action.ReadValue<Vector2>().y;

        if (inputDirection.x < 0)
        {
            animationController.SetBool("movingLeft", true);
            animationController.SetBool("movingRight", false);
        }
        else if (inputDirection.x > 0)
        {
            animationController.SetBool("movingLeft", false);
            animationController.SetBool("movingRight", true);
        }
        else
        {
            animationController.SetBool("movingLeft", false);
            animationController.SetBool("movingRight", false);
        }
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

        if (rigidBody.velocity.magnitude < 1)
        {
            animationController.SetFloat("walkingSpeed", 0);
            animationController.SetBool("isMoving", false);
        }
        else
        {
            if (Vector3.Dot(transform.forward, Vector3.Normalize(rigidBody.velocity)) > 0)
            {
                animationController.SetBool("isMoving", true);
                if (inputDirection.x < 0)
                    animationController.SetFloat(
                        "walkingSpeed",
                        (rigidBody.velocity.magnitude / 5)
                    );
                else
                    animationController.SetFloat(
                        "walkingSpeed",
                        -(rigidBody.velocity.magnitude / 5)
                    );
            }
            else
            {
                animationController.SetBool("isMoving", true);
                animationController.SetFloat("walkingSpeed", (rigidBody.velocity.magnitude / 5));
            }
        }

        // Handle Attack Cooldown
        if (!canAttack)
        {
            attackCooldownTimer += Time.deltaTime;
            if (attackCooldownTimer > 0.5)
                animationController.ResetTrigger("Attack");
            if (attackCooldownTimer >= attackCooldown)
            {
                canAttack = true;
                attackCooldownTimer = 0;
            }
        }

        // Handle Attack
        if (canAttack && grounded && fire.action.triggered)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
            canAttack = false;
            animationController.SetTrigger("Attack");

            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].gameObject.CompareTag("Enemy"))
                {
                    hitColliders[i].gameObject.GetComponent<Entity>().AddHealth(attackDamage);
                }
            }
        }
    }

    void Jump()
    {
        if (grounded)
        {
            grounded = false;
            animationController.SetBool("isGrounded", false);
            if (camEffects.inEffect == false)
                camEffects.PlayImpact(new Vector3(0, 0.3f, 0));
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
    void OnCollisionEnter(Collision other)
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
                if (
                    camEffects.inEffect == false
                    && animationController.GetBool("isGrounded") == false
                )
                {
                    camEffects.PlayImpact(new Vector3(0, -0.3f, 0));
                    Instantiate(
                        groundParticle,
                        other.contacts[i].point + new Vector3(0, 0.3f, 0),
                        Quaternion.identity
                    );
                }
                break;
            }
        }
    }

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
