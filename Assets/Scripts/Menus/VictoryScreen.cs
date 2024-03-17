using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour {
    public void BackToMainMenu() {
        SceneManager.LoadScene(0);
    }

    GameManager GM;

    public TextMeshProUGUI[] StatsText;

    void Start() {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        GM.GameEnded = true;
        StatsText[0].text = "Time Played: " + (int)GM.TimePlayed/60 + ":" + (int)GM.TimePlayed%60;
        StatsText[1].text = "Spells Cast: " + GM.SpellsCast;
        StatsText[2].text = "Enemies Slain: " + GM.EnemiesKilled;
        StatsText[3].text = "Items Grabbed: " + GM.ItemsGrabbed;
    }
}