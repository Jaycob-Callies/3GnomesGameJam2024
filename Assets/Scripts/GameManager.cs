using UnityEngine;
public class GameManager : MonoBehaviour {
    public GameObject Spellbook; //the spellbook gameobject that drops from enemies
    public GameObject DroppedHearts; //the Heart gameobject that drops from enemies
    public GameObject[] Enemies; //the Enemies array

    public int[] CurrentSpells = new int[3]; // this is how many spells you can hold and which IDs are in what spots
    public GameObject[] SpellObjects;

    public int HP;

    public int GrantSpell() {
        return Random.Range(1, 6); // the second number must be 1 higher than the ammount of spells
    }
}