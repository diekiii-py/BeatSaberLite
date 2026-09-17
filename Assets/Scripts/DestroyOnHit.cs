using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DestroyOnHit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("sword")) return;

        MovingHit hit = other.GetComponentInParent<MovingHit>();
        if (hit == null) return;

        Destroy(hit.gameObject);
    }
}
    
