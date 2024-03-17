using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour {
    public void BackToMainMenu() {
        SceneManager.LoadScene(0);
    }
}