using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    public GameObject PauseScreen;

    public static bool GameIsPaused = false;

    GameManager GM;
    private void Start() {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>(); 
    }
    private void Update() {
        if(GM.GameEnded == false) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                if (GameIsPaused) {
                    Resume();
                }
                else {
                    Pause();
                }
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
        this.GM.ResetStats();
        Time.timeScale = 1;
        GameIsPaused = false;
        SceneManager.LoadScene(0);
    }
}