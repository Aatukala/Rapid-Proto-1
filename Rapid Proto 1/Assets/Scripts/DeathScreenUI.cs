using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreenUI : MonoBehaviour
{
    public void ReturnToMenu()
    {
        // Palautetaan Time.timeScale, jos se on 0
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
