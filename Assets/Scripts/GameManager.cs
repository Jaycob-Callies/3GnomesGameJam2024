using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    [Header("Prefabs")]
    public GameObject Spellbook; //the spellbook gameobject that drops from enemies
    public GameObject DroppedHearts; //the Heart gameobject that drops from enemies
    public GameObject[] SpellObjects;

    [Header("Info")]
    public int HP = 3;
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
        return UnityEngine.Random.Range(1, 7); // the second number must be 1 higher than the ammount of spells
    }
	public void ResetStats(){ 
        TimePlayed = 0f;
        SpellsCast = EnemiesKilled = ItemsGrabbed = 0;
        LevelReached = 1;
	}
	public void takeDamage(int damage)
    {
		this.HP -= Mathf.Abs(damage);
		GameObject.FindAnyObjectByType<HeartHolder>().UpdateHearts();
		if (this.HP <= 0)
		{
			GameObject.FindAnyObjectByType<LoseScreen>(FindObjectsInactive.Include).GameOver();
		}
	}

	public void gainHealth(int health, bool setter = false)
    {
        this.HP += Mathf.Abs(health);
        if (setter)
        {
            this.HP = health;
		}
        this.HP = Mathf.Min(this.HP, 7);
        GameObject.FindAnyObjectByType<HeartHolder>().UpdateHearts();

	}
    public int getHP()
    {
        return this.HP;
    }
}