using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MovingHit : MonoBehaviour
{

    void Update()
    {
        transform.position += transform.forward * 0.5f * Time.delatTime;
    }
    
}