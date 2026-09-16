using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MovingHit : MonoBehaviour
{
    public Vector3 requiredDirection;

    void Update()
    {
        transform.position += transform.forward * 0.5f * Time.deltaTime;
    }
    
}