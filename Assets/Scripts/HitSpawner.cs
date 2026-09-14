using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class HitSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] hitPrefabs;

    [Header("Spawn Points")]
    public Transform[] spawnPoints; //

    public List<Vector3> posRotations = new List<Vector3> {
        new Vector3(0.0f, 0.0f, 0.0f),
        new Vector3(0.0f, 0.0f, 45.0f), 
        new Vector3(0.0f, 0.0f, 90.0f),
        new Vector3(0.0f, 0.0f, 135.0f),
        new Vector3(0.0f, 0.0f, 180.0f),
        new Vector3(0.0f, 0.0f, 225.0f),
        new Vector3(0.0f, 0.0f, 270.0f), 
        new Vector3(0.0f, 0.0f, 310.0f),};
        

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
            Vector3 hitRotate = posRotations[Random.Range(0, posRotations.Length)]; 
            Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
            point.Rotate(hitRotate);

            Instantiate(prefabs, point.position, hitRotate.rotation);

            yield return new WaitForSeconds(1f);
        }
    }
}
