using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnContact : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag != "Player") {
            Destroy(gameObject);
        }
    }

    float TimeAlive;
    public float TimeToLive = 3f;
    void Update() {
        TimeAlive += Time.deltaTime;
        if (TimeAlive > TimeToLive) {
            Destroy(gameObject);
        }
    }
}
