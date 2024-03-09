using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spells : MonoBehaviour { 
    //this is attached to the spells object that drops from enemies

    //this needs to randomly drop from enemies
    //it also needs to give the player a randomized spell (maybe based on the color of the book)

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Player") {
            Debug.Log("Aquired New Spell");
            //something something... give the player a new spell here
            Destroy(gameObject);
        }
    } 
}