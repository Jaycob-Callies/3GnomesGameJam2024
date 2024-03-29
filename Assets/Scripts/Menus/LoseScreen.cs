using UnityEngine;
using TMPro;

public class LoseScreen : MonoBehaviour {

    GameManager GM;

    public TextMeshProUGUI[] StatsText;

    public void GameOver() {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        GM.GameEnded = true;
        gameObject.SetActive(true);
        Time.timeScale = 0;
        StatsText[0].text = "Time Played: " + (int)GM.TimePlayed / 60 + ":" + (int)GM.TimePlayed % 60;
        if ((int)GM.TimePlayed % 60 < 10) {
            StatsText[0].text = "Time Played: " + (int)GM.TimePlayed / 60 + ":0" + (int)GM.TimePlayed % 60;
        }
        StatsText[1].text = "Spells Cast: " + GM.SpellsCast; 
        StatsText[2].text = "Enemies Slain: " + GM.EnemiesKilled; 
        StatsText[3].text = "Items Grabbed: " + GM.ItemsGrabbed;
        StatsText[4].text = "Level Reached: " + GM.LevelReached;
    }
}