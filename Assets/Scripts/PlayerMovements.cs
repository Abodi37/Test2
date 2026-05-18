using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    
    private Animator anim;
    private int speedHash;

    void Start()
    {
        // Cache the animator component and parameter ID for performance
        anim = GetComponent<Animator>();
        speedHash = Animator.StringToHash("Speed");
    }

    void Update()
    {
        // Get WASD / Arrow Key inputs (-1 to 1)
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Calculate movement direction vector
        Vector3 moveDir = new Vector3(moveX, 0f, moveZ).normalized;

        // Determine if player is moving and if they are holding Left Shift to run
        bool isMoving = moveDir.magnitude > 0.1f;
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        // Calculate current animation target speed value
        float currentAnimSpeed = 0f;

        if (isMoving)
        {
            currentAnimSpeed = isRunning ? 2f : 1f;
            
            // Move the actual GameObject
            float currentMoveSpeed = isRunning ? runSpeed : walkSpeed;
            transform.Translate(moveDir * currentMoveSpeed * Time.deltaTime, Space.World);
            
            // Rotate player to face movement direction
            transform.forward = moveDir;
        }

        // Send the speed value directly to the Blend Tree parameter
        anim.SetFloat(speedHash, currentAnimSpeed, 0.1f, Time.deltaTime);
    }
}
