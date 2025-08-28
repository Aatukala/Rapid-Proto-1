using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHearts = 3;
    public int currentHearts;

    public GameObject[] heartIcons;
    public GameObject explosionPrefab; // Räjähdysprefab
    public GameObject deathScreen;

    private PlayerController playerController;

    public float deathDelay = 1f; // viive kuolemaan ja deathscreeniin

    void Start()
    {
        currentHearts = maxHearts;
        playerController = GetComponent<PlayerController>();
        UpdateHeartsUI();
        if (deathScreen != null)
            deathScreen.SetActive(false);
    }

    public void TakeDamage(int amount)
    {
        currentHearts -= amount;
        if (currentHearts < 0) currentHearts = 0;

        UpdateHeartsUI();

        if (currentHearts <= 0)
        {
            StartCoroutine(DieRoutine());
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
            heartIcons[i].SetActive(i < currentHearts);
        }
    }

    IEnumerator DieRoutine()
    {
        // Estetään liikkuminen
        if (playerController != null)
            playerController.enabled = false;

        // Piilotetaan pelaaja
        HidePlayer();

        // Spawnataan räjähdys
        if (explosionPrefab != null)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // Odotetaan viive
        yield return new WaitForSecondsRealtime(deathDelay);

        // Näytetään death screen
        if (deathScreen != null)
            deathScreen.SetActive(true);

        // Pysäytetään peli
        Time.timeScale = 0f;
    }

    void HidePlayer()
    {
        // Piilotetaan kaikki Renderer-komponentit lapsineen
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in renderers)
            rend.enabled = false;

        // Vaihtoehtoisesti voidaan myös deactivate koko pelaaja:
        // gameObject.SetActive(false);
    }

}
