using UnityEngine;
using UnityEngine.SceneManagement;

public class MM : MonoBehaviour {

    public GameObject[] Menus; 

    public void QuitGame() {
        Application.Quit();
        Debug.Log("Quit Game");
    }
    public void LoadGame() {
        if (GameObject.Find("GameManager") == true) {
            GameManager GM = GameObject.Find("GameManager").GetComponent<GameManager>();
            GM.HP = 3;
            GM.CurrentSpells[0] = 0;
            GM.CurrentSpells[1] = 0;
            GM.CurrentSpells[2] = 0;
        }
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