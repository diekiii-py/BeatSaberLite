using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DestroyOnHit : MonoBehaviour
{
    private void OnTriggerEnter(Collider Other)
    {
        Destroy(other.GameObject);
    }
}
    
