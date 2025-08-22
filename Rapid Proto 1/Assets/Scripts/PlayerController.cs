using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 6f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    public float laneChangeSpeed = 10f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isJumping = false;

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
                ChangeLane(-1); // Move left
            else if (moveValue.x > 0)
                ChangeLane(1);  // Move right
        };

        inputActions.Player.Jump.performed += ctx =>
        {
            Jump();
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

        // Reset jump state when grounded
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            isJumping = false;
        }

        // Calculate target Z position for lane
        // REVERSED: Now left lane is positive Z, right lane is negative Z
        float targetZ = (1 - desiredLane) * laneDistance;

        // Calculate lane movement direction
        float laneMovement = (targetZ - transform.position.z) * laneChangeSpeed * Time.deltaTime;

        // Apply gravity
        if (!isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        // Move forward constantly (X-axis for forward movement)
        Vector3 forwardMove = Vector3.right * speed * Time.deltaTime;

        // Lane movement (Z-axis)
        Vector3 laneMove = Vector3.forward * laneMovement;

        // Apply vertical movement (jumping/falling)
        Vector3 verticalMove = Vector3.up * velocity.y * Time.deltaTime;

        // Combine all movements
        controller.Move(forwardMove + laneMove + verticalMove);
    }

    void Jump()
    {
        if (isGrounded && !isJumping)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            isJumping = true;
        }
    }

    void ChangeLane(int direction)
    {
        desiredLane = Mathf.Clamp(desiredLane + direction, 0, laneCount - 1);
        Debug.Log($"Changed lane to: {desiredLane} (0=left, 1=middle, 2=right)");
    }

    private void OnTriggerEnter(Collider other)
    {
        Obstacle obstacle = other.GetComponent<Obstacle>();
        if (obstacle == null) return;

        switch (obstacle.type)
        {
            case ObstacleType.Wall:
            case ObstacleType.Pit:
                if (!isJumping)
                {
                    GameOver("Et hypännyt yli esteen!");
                }
                break;

            case ObstacleType.Pillar:
                if (isJumping)
                {
                    GameOver("Et voi hypätä pilarin yli!");
                }
                break;
        }
    }

    private void GameOver(string reason)
    {
        Debug.Log("Game Over: " + reason);
        // TODO: tähän peli loppuu (UI, restart, jne.)
    }
}