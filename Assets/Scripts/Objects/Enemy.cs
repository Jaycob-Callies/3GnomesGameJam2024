using UnityEngine;
using System.Collections.Generic;

public class SpellSlowTracker
{
    List<Vector2> effects = new List<Vector2>();//x expire time, y multiplier 
    List<Color> colors = new List<Color>();//parallel array
    public void AddEffect(float durationSeconds, float moveSpeedReduction, Color newColor)//moveSpeedReduction between 0 and 1
	{
        effects.Add(new Vector2(UnityEngine.Time.time + durationSeconds, 1.0f - moveSpeedReduction));
        colors.Add(newColor);
    }
    public float getMoveSpeedMultiplier()
	{
        float multiplier = 1f;
        for(int slowIndex = 0; slowIndex < effects.Count; slowIndex++)
		{
            if (effects[slowIndex].x > UnityEngine.Time.time)
			{
                multiplier *= effects[slowIndex].y;

            }
			else
			{
                effects.RemoveAt(slowIndex);
                colors.RemoveAt(slowIndex);
                slowIndex--;
            }
		}
        return multiplier;
	}
    public Color getColor()
	{
        return colors.Count >= 1 ? colors[0] : Color.white;
	}
}
public class Enemy : MonoBehaviour {

    private GameManager GM;
    private TeleporterController TC = null;
    private HeartHolder HH;

    public int HP;
    public float currentSpeed { get { return MoveSpeed * slowTracker.getMoveSpeedMultiplier(); } }
    public float MoveSpeed;
    public int DMG;

    private delegate void changeField();
    private SpellSlowTracker slowTracker = new SpellSlowTracker();

    public void slowEnemy(float slowStrength, float durationSeconds, Color newColor)
	{
        slowTracker.AddEffect(durationSeconds, slowStrength, newColor);
	}

    private void Start() {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        HH = GameObject.Find("HeartHolder").GetComponent<HeartHolder>();
		TC = GameObject.FindFirstObjectByType<TeleporterController>();
	}
    private void Update() {
        //This is literally all the code for making enemies move at the player its just 1 line
        transform.position = Vector3.MoveTowards(transform.position, GameObject.Find("Player").transform.position, currentSpeed * Time.deltaTime);
        this.gameObject.GetComponent<SpriteRenderer>().color = this.slowTracker.getColor();
    }

    public void HitEnemy(int DamageTaken) {
        HP-=DamageTaken;
        if (HP <= 0) {
            GM.EnemiesKilled += 1;
            TC.killedEnemy(this.transform.position);
            this.SendMessageUpwards("killedBoss");

			Destroy(gameObject); //idealy this is more like a death animation and not just *poof*

            if(Random.Range(0,5) == 1) { 
                Instantiate(GM.Spellbook, transform.position, Quaternion.identity);
            }else if (Random.Range(0, 10) == 1) {
                Instantiate(GM.DroppedHearts, transform.position, Quaternion.identity);
            }

		}
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "Player") {
            GM.HP -= DMG;
            HH.CheckHearts();
        }
    }
}