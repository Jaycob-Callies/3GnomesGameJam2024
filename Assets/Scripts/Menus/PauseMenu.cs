using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    public GameObject PauseScreen;

    public static bool GameIsPaused = false;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (GameIsPaused) {
                Resume();
            }
            else {
                Pause();
            }
        }
    }

    public void Resume() {
        PauseScreen.SetActive(false);
        Time.timeScale = 1;
        GameIsPaused = false;
    }

    void Pause() {
        PauseScreen.SetActive(true);
        Time.timeScale = 0;
        GameIsPaused = true;
    }

    public void MM() {
        Time.timeScale = 1;
        GameIsPaused = false;
        SceneManager.LoadScene(0);
    }
}