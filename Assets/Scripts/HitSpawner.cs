using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class HitSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] hitPrefabs;

    [Header("Spawn Points")]
    public Transform[] spawnPoints; //

    public List<List<Vector3>> posRotations = new List<List<Vector3>>
    {
        new List<Vector3> { new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0, -1, 0) },
        new List<Vector3> { new Vector3(0.0f, 0.0f, 45.0f), new Vector3(-1, -1, 0) },
        new List<Vector3> { new Vector3(0.0f, 0.0f, 90.0f), new Vector3(-1, 0, 0) },
        new List<Vector3> { new Vector3(0.0f, 0.0f, 135.0f), new Vector3(-1, 1, 0) },
        new List<Vector3> { new Vector3(0.0f, 0.0f, 180.0f), new Vector3(0, 1, 0) },
        new List<Vector3> { new Vector3(0.0f, 0.0f, 225.0f), new Vector3(1, 1, 0) },
        new List<Vector3> { new Vector3(0.0f, 0.0f, 270.0f), new Vector3(1, 0, 0) },
        new List<Vector3> { new Vector3(0.0f, 0.0f, 310.0f), new Vector3(1, -1, 0) },
    };

    public List<Vector3> rotateInfo;
    public bool isRandom = true;
    public TextAsset beatmapFile;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnHits());
    }
    
    IEnumerator SpawnHits()
    {
        while(isRandom)
        {
            GameObject prefab = hitPrefabs[Random.Range(0, hitPrefabs.Length)];
            List<Vector3> rotationInfo = posRotations[Random.Range(0, posRotations.Count)];
            Vector3 hitRotate = rotationInfo[0];
            Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];

            GameObject spawnedHit = Instantiate(prefab, point.position, Quaternion.Euler(hitRotate));
            MovingHit movingHit = spawnedHit.GetComponent<MovingHit>();
            if (movingHit != null)
            {
                movingHit.requiredDirection = rotationInfo[1];
            }

            yield return new WaitForSeconds(1f);
        }
        
        while(!isRandom)
        {

            if (beatmapFile == null)
            {
                Debug.LogError("Assign a beatmap .txt file to beatmapFile.");
                yield break;
            }

            List<string> beatmapInfo = beatmapFile.text
                .Split(new[] { '\r', '\n' }, System.StringSplitOptions.None)
                .Select(line => line.Trim())
                .ToList();

            int bpm = int.Parse(beatmapInfo[0]);
            int subBeats = int.Parse(beatmapInfo[1]);
            float spawnTime = 60f / (bpm * subBeats);

            foreach (string line in beatmapInfo.Skip(2))
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    string[] values = line.Split(',');
                    int rotationIndex = int.Parse(values[0].Trim());
                    int spawnPointIndex = int.Parse(values[1].Trim());
                    GameObject prefab = hitPrefabs[0];
                    List<Vector3> rotationInfo = posRotations[rotationIndex];
                    Vector3 hitRotate = rotationInfo[0];
                    Transform point = spawnPoints[spawnPointIndex];

                    GameObject spawnedHit = Instantiate(prefab, point.position, Quaternion.Euler(hitRotate));
                    MovingHit movingHit = spawnedHit.GetComponent<MovingHit>();
                    if (movingHit != null)
                    {
                        movingHit.requiredDirection = rotationInfo[1];
                    }
                }

                yield return new WaitForSeconds(spawnTime);
            }
        }
    }
}
