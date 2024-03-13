using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStuff : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f, 0f, -10f); 
    private Vector3 Velocity = Vector3.zero; 
    private float smoothTime = 0.25f; //the distance between the player and the center of the camera basically

    [SerializeField]
    private Transform target; //the player

    void Update() {
        Vector3 targetPos = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref Velocity, smoothTime);
    }
}
