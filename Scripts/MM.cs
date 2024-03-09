using UnityEngine;
using UnityEngine.SceneManagement;

public class MM : MonoBehaviour {

    //theme is Use it or Lose it 

    public GameObject[] Menus; 

    public void QuitGame() {
        Application.Quit();
        Debug.Log("Quit Game");
    }

    public void LoadMainMenu() {
        SceneManager.LoadScene(0);
    }
    public void LoadGame() {
        SceneManager.LoadScene(1);
    }
    public void OpenMM() {
        Menus[1].SetActive(false);
        Menus[0].SetActive(true);
    }
    public void OpenCredits() {
        Menus[0].SetActive(false);
        Menus[1].SetActive(true);
    }
}