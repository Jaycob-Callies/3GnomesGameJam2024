using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOffScreen : MonoBehaviour {

	public List<GameObject> Enemies = new List<GameObject>(); //the Enemies array

	public bool canSpawn = true; //by default its true, but when in certain locations it should be off
    float TimeSinceLastSpawn;
    int ranDelay = 0;

    void Start() { 
        //GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update() {
        TimeSinceLastSpawn += Time.deltaTime;
        if(TimeSinceLastSpawn >= ranDelay && canSpawn==true) {
            ranDelay = Random.Range(3,8);
            TimeSinceLastSpawn = 0;

            GameObject selectedToSpawn = Enemies[Random.Range(0, Enemies.Count)];

			//right now it only spawns a single type of enemy because it just has [0] written
			for (int i = 0; i < Random.Range(1, 4); i++) {
                Vector2 spawnLocation = Random.insideUnitCircle.normalized * Camera.main.orthographicSize * 2f;
                Instantiate(selectedToSpawn, transform.position + new Vector3(spawnLocation.x, spawnLocation.y), Quaternion.identity);
               
            }
        } 
    }
}