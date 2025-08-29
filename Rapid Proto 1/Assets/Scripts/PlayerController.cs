using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float baseSpeed = 6f;
    public float maxSpeed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    public float laneChangeSpeed = 10f;
    public float laneDistance = 4f; 
    private int laneCount = 3;

    [Header("Progression")]
    public Slider progressBar;
    public Transform finishLine;   // viittaus maaliviivaan
    private float startX;          // aloituspaikka X-akselilla
    private float totalDistance;   // start → maaliviiva

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isJumping = false;
    private PlayerInputActions inputActions;

    private int desiredLane = 1; // 0 = left, 1 = middle, 2 = right
    private float currentSpeed;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputActions = new PlayerInputActions();

        inputActions.Player.Move.performed += ctx =>
        {
            Vector2 moveValue = ctx.ReadValue<Vector2>();
            if (moveValue.x < 0) ChangeLane(-1);
            else if (moveValue.x > 0) ChangeLane(1);
        };

        inputActions.Player.Jump.performed += ctx => Jump();
    }

    void Start()
    {
        startX = transform.position.x;
        if (finishLine != null)
            totalDistance = Mathf.Abs(startX - finishLine.position.x);
    }

    void OnEnable() => inputActions.Player.Enable();
    void OnDisable() => inputActions.Player.Disable();

    void Update()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            isJumping = false;
        }

        // Lane movement
        float targetZ = (desiredLane - 1) * laneDistance;
        float newZ = Mathf.MoveTowards(transform.position.z, targetZ, laneChangeSpeed * Time.deltaTime);
        Vector3 laneMove = new Vector3(0, 0, newZ - transform.position.z);

        // Forward speed scaling
        float progressSpeed = Mathf.Clamp01(Mathf.Abs(startX - transform.position.x) / totalDistance);
        currentSpeed = Mathf.Lerp(baseSpeed, maxSpeed, progressSpeed);
        Vector3 forwardMove = Vector3.left * currentSpeed * Time.deltaTime;

        // Gravity + jump
        if (!isGrounded)
            velocity.y += gravity * Time.deltaTime;
        Vector3 verticalMove = Vector3.up * velocity.y * Time.deltaTime;

        // Apply movement
        controller.Move(forwardMove + laneMove + verticalMove);

        // Update progress bar
        if (progressBar != null && finishLine != null)
        {
            float distanceTravelled = Mathf.Abs(startX - transform.position.x);
            progressBar.value = Mathf.Clamp01(distanceTravelled / totalDistance);
        }
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
    }

    public float GetCurrentSpeed() => currentSpeed;
    public int GetDesiredLane() => desiredLane;
    public float GetLaneDistance() => laneDistance;
<<<<<<< Updated upstream
    public float GetDistanceTravelled() => Mathf.Abs(transform.position.x - startX);

    public float GetSpeed()
    {
        return baseSpeed;
    }

    public void SetSpeed(float newSpeed)
    {
        baseSpeed = newSpeed;
    }
=======
>>>>>>> Stashed changes
}
