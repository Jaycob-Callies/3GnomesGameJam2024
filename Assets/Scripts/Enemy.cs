using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public int HP;
    public float MoveSpeed;

    public void Update()
    {
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }

}