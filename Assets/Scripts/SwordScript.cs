using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SwordScript : MonoBehaviour
{
    private Vector3 lastPos;
    private Vector3 velocity;

    public string swordColorTag = "Red";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        velocity = (transform.position - lastPos) / Time.deltaTime;
        lastPos = transform.position;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        MovingHit hit = other.GetComponent<MovingHit>();
        if (hit == null) return;


        if(other.tag != swordColorTag)
        {
            Debug.Log("Wrong color");
            return;
        }

        Vector3 swingDir = velocity.normalized;

        float dot = Vector3.Dot(swingDir, hit.requiredDirection);

        if (dot > 0.7f)
        {
            Debug.Log("Correct slice!");
            Destroy(hit.gameObject);
        }
        else
        {
            Debug.Log("Wrong direction...");
        }
    }
}
