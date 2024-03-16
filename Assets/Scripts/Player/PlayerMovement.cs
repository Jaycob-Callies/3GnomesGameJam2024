using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Camera cam;
    private TerrainCollisionController collisionController = null;

    Vector2 movement;
    Vector2 mousePos;

    private void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    void Update() {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate() {
		if (collisionController == null)
		{
			collisionController = GameObject.FindAnyObjectByType<TerrainCollisionController>();
		}
		Vector2 collisionMovement = collisionController.getTerrainVelocity(this.transform.position, this.movement.normalized);
        rb.MovePosition(rb.position + collisionMovement * moveSpeed * Time.fixedDeltaTime) ;
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }
}