using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Speeds")]
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float rotationSpeed = 10f; // Smooth rotation speed multiplier

    [Header("References")]
    public CharacterController controller;
    public Transform cameraTransform; // Drag your Main Camera here
    public ParticleSystem runParticles;

    private Animator anim;
    private int speedHash;
    private float verticalVelocity; // Handles gravity

    void Start()
    {
        anim = GetComponent<Animator>();
        speedHash = Animator.StringToHash("Speed");

        // Automatically fetch components if they aren't assigned in inspector
        if (controller == null) controller = GetComponent<CharacterController>();
        if (cameraTransform == null && Camera.main != null) cameraTransform = Camera.main.transform;

        if (runParticles != null && runParticles.isPlaying) runParticles.Stop();
    }

    void Update()
    {
        // 1. Get raw input from WASD / Left Stick
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        // 2. Get camera directions and flatten them (ignore height tilts)
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        // 3. Calculate absolute direction vector relative to camera
        Vector3 moveDir = (camForward * moveZ + camRight * moveX).normalized;

        bool isMoving = moveDir.magnitude > 0.1f;
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentAnimSpeed = 0f;

        // 4. Handle horizontal movement and rotation
        Vector3 velocity = Vector3.zero;

        if (isMoving)
        {
            currentAnimSpeed = isRunning ? 2f : 1f;
            float currentMoveSpeed = isRunning ? runSpeed : walkSpeed;

            // Multiply direction vector by chosen speed
            velocity = moveDir * currentMoveSpeed;

            // Smoothly rotate character to face the movement direction
            // Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // 5. Apply simple Gravity so character stays grounded
        if (controller.isGrounded)
        {
            verticalVelocity = -0.5f; // Small constant downward force to stay stuck to slopes
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
        velocity.y = verticalVelocity;

        // 6. Execute final frame movement using CharacterController
        controller.Move(velocity * Time.deltaTime);

        // 7. Update animator parameter and particles
        anim.SetFloat(speedHash, currentAnimSpeed, 0.1f, Time.deltaTime);
        HandleParticles(isMoving && isRunning);
    }

    void HandleParticles(bool shouldRunParticles)
    {
        if (runParticles == null) return;

        if (shouldRunParticles)
        {
            if (!runParticles.isPlaying) runParticles.Play();
        }
        else
        {
            if (runParticles.isPlaying) runParticles.Stop();
        }
    }
}
