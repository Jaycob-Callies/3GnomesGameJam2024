using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float projectileSpeed = 1f;
    public int DMG;
    [HideInInspector]
    public GameManager GM = null;
    private bool marktodelete = false;

    [HideInInspector]
    public TerrainCollisionController TCC = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = this.transform.position + ((this.transform.rotation * Vector3.up) * this.projectileSpeed * Time.deltaTime);
        if (this.TCC.terrainSpeedAtPoint(this.transform.position) <= -.9f || marktodelete)
		{
            GameObject.DestroyImmediate(this.gameObject);
        }
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
        if (collision.gameObject.tag == "Player")
        {
            if (GM == null) {
                GM = GameObject.FindFirstObjectByType<GameManager>();
			}
            GM.takeDamage(DMG);
            marktodelete = true;
        }
    }

}
