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
            if (GM.HP < 7) {
                //This is where the player Gets new hearts
                HH.Hearts[GM.HP].SetActive(true);
                GM.HP += 1;
                Debug.Log("MaxHp = " + GM.HP);
            }
            else {
                Debug.Log("Cant Gain more HP");
            }

            Destroy(gameObject);
        }
    }
}
