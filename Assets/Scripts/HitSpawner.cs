using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class HitSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] hitPrefabs;

    [Header("Spawn Points")]
    public Transform[] spawnPoints; //

    public List<List<Vector3>> posRotations = new List<List<Vector3>> 
    {
        new List<Vector3>((0.0f, 0.0f, 0.0f),(0,-1,0)),
        new List<Vector3>((0.0f, 0.0f, 45.0f),(-1,-1,0)), 
        new List<Vector3>((0.0f, 0.0f, 90.0f),(-1,0,0)),
        new List<Vector3>((0.0f, 0.0f, 135.0f),(-1,1,0)),
        new List<Vector3>((0.0f, 0.0f, 180.0f),(0,1,0)),
        new List<Vector3>((0.0f, 0.0f, 225.0f),(1,1,0)),
        new List<Vector3>((0.0f, 0.0f, 270.0f),(1,0,0)), 
        new List<Vector3>((0.0f, 0.0f, 310.0f),(1,-1,0)),
    };
    
    public List<Vector3> rotateInfo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnHits());
    }
    
    IEnumerator SpawnHits()
    {
        while(true)
        {
            GameObject prefab = hitPrefabs[Random.Range(0, hitPrefabs.Length)];
            List<Vector3> rotateInfo = posRotations[Random.Range(0, posRotations)];
            Vector3 hitRotate = rotateInfo[0]; 
            Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
            point.Rotate(hitRotate);

            Instantiate(prefabs, point.position, hitRotate.rotation);

            yield return new WaitForSeconds(1f);
        }
    }
}
