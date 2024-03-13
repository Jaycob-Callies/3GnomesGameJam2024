using UnityEngine;

public class SpellAttacks : MonoBehaviour {

    //This script is on the Spells Themselves 
    public int Damage;

    public bool SlowEffect;
    public bool StunEffect;
    float timer;

    bool isEffect;

    private void Update()
    {
        if(isEffect == true && StunEffect)
        {
            timer += Time.deltaTime;

        }
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.tag == "Enemy") {
            col.gameObject.GetComponent<Enemy>().HitEnemy(Damage);
            if (SlowEffect == true)
            {
                col.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.5f, 0.2f);
                float MS = col.gameObject.GetComponent<Enemy>().MoveSpeed;
                col.gameObject.GetComponent<Enemy>().MoveSpeed = MS/ 2;
            }
            if (StunEffect == true) {
                isEffect = true;
                col.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 0.0f);
                col.gameObject.GetComponent<Enemy>().MoveSpeed = 0;
            }
        }
    }
}