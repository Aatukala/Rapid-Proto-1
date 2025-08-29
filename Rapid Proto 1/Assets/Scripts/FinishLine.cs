using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    public GameObject winCanvas; // Drag & Drop Unityssä

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("FINISH! You Win!");
            Time.timeScale = 0f; // pysäytetään peli
            if (winCanvas != null)
                winCanvas.SetActive(true);
        }
    }

    // Tätä voi kutsua napista (Return to Menu)
    public void ReturnToMenu()
    {
        Time.timeScale = 1f; // jatketaan aikaa
        SceneManager.LoadScene("MainMenu"); // HUOM: vaihda oikea menu-scene nimi
    }
}
