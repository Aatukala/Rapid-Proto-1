using UnityEngine;
using UnityEngine.InputSystem; // Needed for new Input System

public class PlayerController : MonoBehaviour
{
    public float speed = 6f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    private PlayerInputActions inputActions;

    private int desiredLane = 1; // 0 = left, 1 = middle, 2 = right
    public float laneDistance = 4f; // Distance between lanes
    private int laneCount = 3;

    void Awake()
    {
        controller = GetComponent<CharacterController>(); 
        inputActions = new PlayerInputActions();

        inputActions.Player.Move.performed += ctx =>
        {
            Vector2 moveValue = ctx.ReadValue<Vector2>();
            if (moveValue.x < 0)
                ChangeLane(-1);
            else if (moveValue.x > 0)
                ChangeLane(1);
        };
        inputActions.Player.Jump.performed += ctx => Jump();
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Move towards the desired lane position (Z-axis)
        Vector3 targetPosition = transform.position.x * Vector3.right +
                                 (desiredLane - 1) * laneDistance * Vector3.forward;
        Vector3 moveDirection = targetPosition - transform.position;
        moveDirection.y = 0; // Only move horizontally

        // Constant movement forward
        Vector3 forwardMove = transform.forward * speed;

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Combine all movement
        Vector3 totalMove = moveDirection.normalized * speed;
        totalMove += forwardMove;
        totalMove.y = velocity.y;

        controller.Move(totalMove * Time.deltaTime);
    }
    
    void Jump()
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void ChangeLane(int direction)
    {
        desiredLane = Mathf.Clamp(desiredLane + direction, 0, laneCount - 1);
    }    
}