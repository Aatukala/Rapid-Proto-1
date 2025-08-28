using UnityEngine;
using UnityEngine.InputSystem;

public class SpearThrower : MonoBehaviour
{
    public GameObject spearPrefab;
    public float throwForce = 15f;
    public float spearLifetime = 3f;
    public Transform throwPoint; // Assign this in Inspector for precise positioning

    private PlayerInputActions inputActions;
    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();

        if (playerController == null)
        {
            Debug.LogError("PlayerController not found on the same GameObject!");
            return;
        }

        inputActions = new PlayerInputActions();
        inputActions.Player.Throw.performed += ctx => ThrowSpear();
        inputActions.Player.Enable();
    }

    void OnDisable()
    {
        if (inputActions != null)
        {
            inputActions.Player.Disable();
        }
    }

    public void ThrowSpear()
    {
        if (spearPrefab == null)
        {
            Debug.LogWarning("Spear prefab not assigned!");
            return;
        }

        if (playerController == null)
        {
            Debug.LogError("PlayerController reference is null!");
            return;
        }

        // Calculate spawn position based on current lane
        Vector3 spawnPosition = CalculateSpawnPosition();

        // Instantiate spear
        GameObject spear = Instantiate(spearPrefab, spawnPosition, Quaternion.identity);

        // Get spear rigidbody and throw it FORWARD (right direction in endless runner)
        Rigidbody rb = spear.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Throw in the direction the player is moving (forward = right in X-axis)
            rb.linearVelocity = Vector3.right * throwForce;

            // Rotate spear to face the throw direction
            spear.transform.rotation = Quaternion.LookRotation(Vector3.right);
        }
        else
        {
            Debug.LogWarning("Spear prefab has no Rigidbody!");
        }

        // Destroy spear after lifetime
        Destroy(spear, spearLifetime);
    }

    private Vector3 CalculateSpawnPosition()
    {
        if (playerController == null)
        {
            Debug.LogError("PlayerController is null in CalculateSpawnPosition!");
            return transform.position;
        }

        // Get current lane from PlayerController
        int currentLane = playerController.GetDesiredLane();
        float laneDistance = playerController.GetLaneDistance();

        // Calculate Z position based on lane
        float laneZ = (1 - currentLane) * laneDistance;

        // Use throwPoint if assigned, otherwise use player position with offset
        Vector3 spawnPos;
        if (throwPoint != null)
        {
            spawnPos = throwPoint.position;
            spawnPos.z = laneZ; // Override Z with lane position
        }
        else
        {
            // Fallback: position in front of player on the correct lane
            spawnPos = transform.position + Vector3.right * 2f; // 2 units in front
            spawnPos.z = laneZ; // Set the correct lane position
        }

        return spawnPos;
    }
}