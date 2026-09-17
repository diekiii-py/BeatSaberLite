using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using EzySlice;

public class SwordScript : MonoBehaviour
{
    private Vector3 lastPos;
    private Vector3 velocity;

    public string swordColorTag = "Red";
    public Material crossSectionMaterial;
    public Material redCrossSectionMaterial;
    public Material purpleCrossSectionMaterial;
    public float pieceExplosionForce = 2.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastPos = transform.position;
    }

    void Update()
    {
        velocity = (transform.position - lastPos) / Time.deltaTime;
        velocity[2] = 0; 
        lastPos = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        MovingHit hit = other.GetComponentInParent<MovingHit>();
        if (hit == null) return;

        if (!hit.gameObject.CompareTag(swordColorTag))
        {
            Debug.Log("Wrong color");
            return;
        }

        if (velocity.sqrMagnitude < 0.01f) return;

        float angle = Vector3.Angle(velocity, hit.requiredDirection);

        if (angle <= 45f)
        {
            Debug.Log("Correct slice!");
            SliceHit(other, hit);
        }
        else
        {
            Debug.Log("Wrong direction...");
        }
    }

    private void SliceHit(Collider other, MovingHit hit)
    {
        Vector3 swingDirection = velocity.normalized;
        Vector3 bladeEdgeDirection = transform.up;
        Vector3 planeNormal = Vector3.Cross(swingDirection, bladeEdgeDirection).normalized;

        if (planeNormal.sqrMagnitude < 0.0001f)
        {
            Destroy(hit.gameObject);
            return;
        }

        Vector3 planePosition = other.ClosestPoint(transform.position);
        Material sliceMaterial = crossSectionMaterial;
        if (hit.gameObject.CompareTag("Red"))
        {
            sliceMaterial = redCrossSectionMaterial;
        }
        else if (hit.gameObject.CompareTag("Purple"))
        {
            sliceMaterial = purpleCrossSectionMaterial;
        }

        GameObject[] pieces = hit.gameObject.SliceInstantiate(
            planePosition,
            planeNormal,
            sliceMaterial);

        if (pieces == null || pieces.Length == 0)
        {
            Destroy(hit.gameObject);
            return;
        }

        foreach (GameObject piece in pieces)
        {
            MeshFilter meshFilter = piece.GetComponent<MeshFilter>();
            if (meshFilter == null || meshFilter.sharedMesh == null) continue;

            MeshCollider meshCollider = piece.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = meshFilter.sharedMesh;
            meshCollider.convex = true;

            Rigidbody rigidbody = piece.AddComponent<Rigidbody>();
            rigidbody.mass = 1f;
            piece.AddComponent<DestroyAfterSeconds>();
            rigidbody.AddForce(planeNormal * pieceExplosionForce, ForceMode.Impulse);
        }

        Destroy(hit.gameObject);
    }
}
