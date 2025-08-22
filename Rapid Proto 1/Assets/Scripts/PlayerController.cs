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
    private bool isJumping = false;

    private PlayerInputActions inputActions;

    private int desiredLane = 1; // 0 = left, 1 = middle, 2 = right
    public float laneDistance = 4f; // Distance between lanes
    private int laneCount = 3;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputActions = new PlayerInputActions();

        // Liikkuminen sivuille (lane switch)
        inputActions.Player.Move.performed += ctx =>
        {
            Vector2 moveValue = ctx.ReadValue<Vector2>();
            if (moveValue.x < 0)
                ChangeLane(-1);
            else if (moveValue.x > 0)
                ChangeLane(1);
        };

        // Hyppy
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

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            isJumping = false; // Palautetaan hypyn tila
        }

        // Lasketaan target position lane-logiikalla (x-akseli)
        Vector3 targetPosition = transform.position.z * Vector3.forward +
                                 Vector3.up * transform.position.y +
                                 (desiredLane - 1) * laneDistance * Vector3.right;

        Vector3 moveDirection = targetPosition - transform.position;
        moveDirection.y = 0; // Ei vaikuta hyppyyn



        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Constant forward movement (z-suunnassa)
        Vector3 forwardMove = Vector3.forward * speed * Time.deltaTime;
        controller.Move(forwardMove);
    

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
            isJumping = true;
        }
    }

    void ChangeLane(int direction)
    {
        desiredLane = Mathf.Clamp(desiredLane + direction, 0, laneCount - 1);
    }

    // Esteiden käsittely
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
