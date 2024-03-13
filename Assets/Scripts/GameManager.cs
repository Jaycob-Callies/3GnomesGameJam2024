using UnityEngine;
public class GameManager : MonoBehaviour {
    [Header("Prefabs")]
    public GameObject Spellbook; //the spellbook gameobject that drops from enemies
    public GameObject DroppedHearts; //the Heart gameobject that drops from enemies
    public GameObject[] Enemies; //the Enemies array
    public GameObject[] SpellObjects;

    [Header("Info")]
    public int HP;
    public bool GameEnded;

    public int[] CurrentSpells = new int[3]; // this is how many spells you can hold and which IDs are in what spots

    //Stats for the lose screen
    [Header("Statistics")] 
    public float TimePlayed;
    public int SpellsCast;
    public int EnemiesKilled;
    public int ItemsGrabbed;
    public int LevelReached = 1;

    public int GrantSpell() {
        return Random.Range(1, 6);
    }
}