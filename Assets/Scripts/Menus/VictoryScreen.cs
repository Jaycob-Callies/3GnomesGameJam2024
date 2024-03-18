using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour {

    GameManager GM;

    public TextMeshProUGUI[] StatsText;

    public void BackToMainMenu() {
        GM.ResetStats();
        SceneManager.LoadScene(0);
    }

    void Start() {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        GM.GameEnded = true;
        StatsText[0].text = "Time Played: " + (int)GM.TimePlayed/60 + ":" + (int)GM.TimePlayed%60;
        if((int)GM.TimePlayed % 60 < 10) { 
            StatsText[0].text = "Time Played: " + (int)GM.TimePlayed / 60 + ":0" + (int)GM.TimePlayed % 60;
        }
        StatsText[1].text = "Spells Cast: " + GM.SpellsCast;
        StatsText[2].text = "Enemies Slain: " + GM.EnemiesKilled;
        StatsText[3].text = "Items Grabbed: " + GM.ItemsGrabbed;
    }
}