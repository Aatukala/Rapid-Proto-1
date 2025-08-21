using UnityEngine;
using UnityEngine.SceneManagement; // tarvitaan Sceneen siirtymiseen

public class MainMenu : MonoBehaviour
{
    // Käynnistää pelin lataamalla seuraavan scenen
    public void PlayGame()
    {
        // Jos käytät build settingsissä useampaa sceneä,
        // tämä lataa seuraavan indeksissä olevan scenen
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        // Vaihtoehto: SceneManager.LoadScene("SceneNimi");
    }

    // Sulkee pelin
    public void QuitGame()
    {
        Debug.Log("Peli suljetaan..."); // näkyy vain editorissa
        Application.Quit(); // toimii buildatussa versiossa
    }
}
