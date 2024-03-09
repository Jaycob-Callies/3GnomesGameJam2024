using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectile : MonoBehaviour {

    public GameObject bulletPrefab;

    [HideInInspector]
    public float projectileForce = 4f; // Bullet Velocity

    [HideInInspector]
    public float attackSpeed = 0.5f; // Attack Speed (Lower Number = faster)
    private float ShootDelay;

    void Update()
    {
        if (ShootDelay > -1)
        {
            ShootDelay -= Time.deltaTime;
        }

        if (Input.GetButton("Fire1"))
        {
            if (ShootDelay < 0)
            {
                Shoot();
            }
        }
    }

    void Shoot()
    {
        //Shooting for the default character
        ShootDelay = attackSpeed;

        //this part spawns the projectile
        GameObject projectile = Instantiate(bulletPrefab, transform.position, transform.rotation);

        //Moves the projectile
        projectile.GetComponent<Rigidbody2D>().AddForce(transform.up * projectileForce, ForceMode2D.Impulse);

    }
}
