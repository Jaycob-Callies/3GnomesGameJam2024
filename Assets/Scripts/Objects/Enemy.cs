using UnityEngine;

public class Enemy : MonoBehaviour {

    private GameManager GM;
    private HeartHolder HH;

    public int HP;
    public float MoveSpeed;
    public int DMG;

    private void Start() {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        HH = GameObject.Find("HeartHolder").GetComponent<HeartHolder>();
        
    }

    private void Update() {
        //This is literally all the code for making enemies move at the player its just 1 line
        transform.position = Vector3.MoveTowards(transform.position, GameObject.Find("Player").transform.position, MoveSpeed * Time.deltaTime);
    }


    public void HitEnemy(int DamageTaken) {
        HP-=DamageTaken;
        if (HP <= 0) {
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