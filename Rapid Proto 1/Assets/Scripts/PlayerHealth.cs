using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHearts = 3;
    public int currentHearts;

    public GameObject[] heartIcons;       // UI heart GameObjectit
    public GameObject deathScreen;        // Death screen UI
    public ParticleSystem deathExplosion; // Räjähdys prefab

    private PlayerController playerController;

    void Start()
    {
        currentHearts = maxHearts;
        playerController = GetComponent<PlayerController>();
        UpdateHeartsUI();

        if (deathScreen != null)
            deathScreen.SetActive(false); // piilotetaan death screen alussa
    }

    public void TakeDamage(int amount)
    {
        currentHearts -= amount;
        if (currentHearts < 0) currentHearts = 0;

        UpdateHeartsUI();

        if (currentHearts <= 0)
            Die();
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
            heartIcons[i].SetActive(i < currentHearts);
        }
    }

    void Die()
    {
        Debug.Log("Player died!");

        // Estetään pelaajan liikkuminen
        if (playerController != null)
            playerController.enabled = false;

        // Spawnataan räjähdys
        if (deathExplosion != null)
            Instantiate(deathExplosion, transform.position, Quaternion.identity);

        // Näytetään death screen
        if (deathScreen != null)
            deathScreen.SetActive(true);

        // Pysäytetään peli kokonaan
        Time.timeScale = 0f;
    }
}
