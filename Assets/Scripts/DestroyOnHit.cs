using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DestroyOnHit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        MovingHit hit = other.GetComponentInParent<MovingHit>();
        if (hit != null)
        {
            Destroy(hit.gameObject);
        }
    }
}
    
