using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MovingHit : MonoBehaviour
{
    public Vector3 requiredDirection;
    public float moveSpeed = -5f;
    public static float sharedMoveSpeed = -5f;

    void Update()
    {
        transform.position += transform.forward * sharedMoveSpeed * Time.deltaTime;
    }
    
}