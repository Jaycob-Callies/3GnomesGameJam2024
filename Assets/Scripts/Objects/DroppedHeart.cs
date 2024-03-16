using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedHeart : MonoBehaviour {

    GameManager GM;
    HeartHolder HH;

    void Start() {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>(); 
        HH = GameObject.Find("HeartHolder").GetComponent<HeartHolder>(); 
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player") {
            GM.gainHealth(1);
            GM.ItemsGrabbed += 1;
            Destroy(gameObject);
        }
    }
}
