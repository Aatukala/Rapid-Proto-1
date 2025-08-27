using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHearts = 3;
    public int currentHearts;

    public GameObject[] heartIcons; // käytetään GameObjecteja, ei pelkkiä spriteitä

    private PlayerController playerController;

    void Start()
    {
        currentHearts = maxHearts;
        playerController = GetComponent<PlayerController>();
        UpdateHeartsUI();
    }

    public void TakeDamage(int amount)
    {
        currentHearts -= amount;
        if (currentHearts < 0) currentHearts = 0;

        UpdateHeartsUI();

        if (currentHearts <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHearts += amount;
        if (currentHearts > maxHearts) currentHearts = maxHearts;

        UpdateHeartsUI();
    }

    void UpdateHeartsUI()
    {
        for (int i = 0; i < heartIcons.Length; i++)
        {
            if (i < currentHearts)
                heartIcons[i].SetActive(true);
            else
                heartIcons[i].SetActive(false);
        }
    }

    void Die()
    {
        Debug.Log("Player died!");

        // Estetään liikkuminen
        if (playerController != null)
            playerController.enabled = false;

        // Pysäytetään peli
        Time.timeScale = 0f;

        // tänne voit lisätä animaation, respawnin tai Game Over -menun
    }
}
