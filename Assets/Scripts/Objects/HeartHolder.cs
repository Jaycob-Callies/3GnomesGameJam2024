using UnityEngine;
using UnityEngine.SceneManagement;
public class HeartHolder : MonoBehaviour {
    public GameObject[] Hearts;
    public GameObject LoseScreen;
    GameManager GM;
    private void Start() {
        //This is so the hearts show up when you go to another scene 
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
		UpdateHearts();
    }

    public void UpdateHearts() {
		for (int i = 0; i < 7; i++) {
            if (i < GM.getHP())
            {
				Hearts[i].SetActive(true);
			}
            else
			{
				Hearts[i].SetActive(false);
			}
        }
    }
}