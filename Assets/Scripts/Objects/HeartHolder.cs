using UnityEngine;
using UnityEngine.SceneManagement;
public class HeartHolder : MonoBehaviour {
    public GameObject[] Hearts;
    GameManager GM;
    private void Start() {
        //This is so the hearts show up when you go to another scene 
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        CheckHearts();
    }

    public void CheckHearts() {
        for (int i = 0; i < 7; i++) {
            Hearts[i].SetActive(false);
        }
        for (int i = 0; i < GM.HP; i++) {
            Hearts[i].SetActive(true);
        }
        if(GM.HP == 0) {
            //YOU LOSE SCREEN
            SceneManager.LoadScene(0);
        }
    }
}