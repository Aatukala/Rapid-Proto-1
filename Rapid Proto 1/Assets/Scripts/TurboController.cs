using UnityEngine;
using UnityEngine.InputSystem;

public class TurboController : MonoBehaviour
{
    public int turboCost = 100;
    public float turboDuration = 5f;
    public float turboSpeedMultiplier = 2f;
    public GameObject flameEffect;

    private PlayerInputActions inputActions;
    private PlayerController playerController;
    private bool isTurboActive = false;
    private float turboTimer = 0f;
    private float originalSpeed;

    void Start()
    {
        playerController = GetComponent<PlayerController>();

        if (playerController == null)
        {
            Debug.LogError("PlayerController not found!");
            return;
        }

        // Hide flame effect at start
        if (flameEffect != null)
        {
            flameEffect.SetActive(false);
        }

        originalSpeed = playerController.GetSpeed();

        inputActions = new PlayerInputActions();
        inputActions.Player.Turbo.performed += ctx => ActivateTurbo();
        inputActions.Player.Enable();
    }

    void Update()
    {
        if (isTurboActive)
        {
            turboTimer -= Time.deltaTime;

            if (turboTimer <= 0f)
            {
                DeactivateTurbo();
            }
        }
    }

    void OnDisable()
    {
        if (inputActions != null)
        {
            inputActions.Player.Disable();
        }
    }

    public void ActivateTurbo()
    {
        // Prevent activation if already active or not enough score
        if (isTurboActive)
        {
            Debug.Log("Turbo is already active!");
            return;
        }

        if (ScoreManager.Instance == null)
        {
            Debug.LogError("ScoreManager not found!");
            return;
        }

        int currentScore = ScoreManager.Instance.GetScore();
        if (currentScore < turboCost)
        {
            Debug.Log($"Not enough score for turbo! Need: {turboCost}, Have: {currentScore}");
            return;
        }

        // Deduct score and activate turbo
        ScoreManager.Instance.DeductScore(turboCost);

        isTurboActive = true;
        turboTimer = turboDuration;
        playerController.SetSpeed(originalSpeed * turboSpeedMultiplier);

        // Show flame effect
        if (flameEffect != null)
        {
            flameEffect.SetActive(true);
        }

        Debug.Log($"Turbo activated! Cost: {turboCost}, Remaining score: {ScoreManager.Instance.GetScore()}");
    }

    private void DeactivateTurbo()
    {
        isTurboActive = false;
        playerController.SetSpeed(originalSpeed);

        // Hide flame effect
        if (flameEffect != null)
        {
            flameEffect.SetActive(false);
        }

        Debug.Log("Turbo deactivated.");
    }

    // Optional: Method to check if player can afford turbo
    public bool CanAffordTurbo()
    {
        if (ScoreManager.Instance == null) return false;
        return ScoreManager.Instance.GetScore() >= turboCost;
    }

    // Optional: Get current turbo cost
    public int GetTurboCost()
    {
        return turboCost;
    }

    public bool IsTurboActive() => isTurboActive;
    public float GetRemainingTurboTime() => turboTimer;
}