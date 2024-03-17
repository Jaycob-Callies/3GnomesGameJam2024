using UnityEngine;

public class SpellAttacks : MonoBehaviour {

    //This script is on the Spells Themselves 
    public int Damage;

    public bool SlowEffect;
    public bool StunEffect;

    private void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.tag == "Enemy") {
            col.gameObject.GetComponent<Enemy>().HitEnemy(Damage);
            if (SlowEffect == true) {
                col.gameObject.GetComponent<Enemy>().slowEnemy(0.5f, 5f, new Color(0.2f, 0.5f, 0.2f));// = MS/ 2;
            }
            if (StunEffect == true) {
                col.gameObject.GetComponent<Enemy>().slowEnemy(1.0f, 2f, new Color(1f, 1f, 0.0f));
            }
        }
    }
}