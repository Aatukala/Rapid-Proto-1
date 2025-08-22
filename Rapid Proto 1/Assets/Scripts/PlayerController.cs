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

        // Calculate target X position for lane
        float targetX = (desiredLane - 1) * laneDistance;
        Vector3 currentPosition = transform.position;
        float newX = Mathf.MoveTowards(currentPosition.x, targetX, speed * Time.deltaTime);

        // Build movement vector: lane (X), gravity (Y), forward (Z)
        Vector3 move = new Vector3(newX - currentPosition.x, 0, speed * Time.deltaTime);

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        move.y = velocity.y * Time.deltaTime;

        controller.Move(move);

        //To be added soon
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