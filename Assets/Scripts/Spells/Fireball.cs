using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

    public int Damage;

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "Enemy") {
            collision.gameObject.GetComponent<Enemy>().HP--;
        }
    } 
}
