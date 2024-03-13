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
        StatsText[0].text = "Time Played: " + (int)GM.TimePlayed; // this should be minutes:seconds
        StatsText[1].text = "Spells Cast: " + GM.SpellsCast; 
        StatsText[2].text = "Enemies Slain: " + GM.EnemiesKilled; 
        StatsText[3].text = "Items Grabbed: " + GM.ItemsGrabbed;
        StatsText[4].text = "Level Reached: " + GM.LevelReached;
    }
}