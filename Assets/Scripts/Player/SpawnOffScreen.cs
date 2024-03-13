using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOffScreen : MonoBehaviour {

    GameManager GM;

    public bool canSpawn = true; //by default its true, but when in certain locations it should be off
    float TimeSinceLastSpawn;
    int ranDelay = 0;

    void Start() { 
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update() {
        TimeSinceLastSpawn += Time.deltaTime;
        if(TimeSinceLastSpawn >= ranDelay && canSpawn==true) {
            ranDelay = Random.Range(3,8);
            TimeSinceLastSpawn = 0;

            #region RandomizingSpawnPosition 
            int ranX = Random.Range(-1, 2);
            int ranY = Random.Range(-1, 2);
            while(ranX == ranY && ranX == 0) { 
                ranX = Random.Range(-1, 2);
                ranY = Random.Range(-1, 2);
            }
            #endregion 

            //right now it only spawns a single type of enemy because it just has [0] written
            for(int i = 0; i < Random.Range(1, 4); i++) {
                if (ranX != 0) { 
                    Instantiate(GM.Enemies[0], transform.position + new Vector3(10 * ranX, (10 * ranY)+i*2), Quaternion.identity);
                }else if (ranY != 0) { 
                    Instantiate(GM.Enemies[0], transform.position + new Vector3((10 * ranX) + i, 10 * ranY), Quaternion.identity);
                }
            }
        } 
    }
}