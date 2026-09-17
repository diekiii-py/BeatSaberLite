using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using TMPro;

public class HitSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] hitPrefabs;

    [Header("Spawn Points")]
    public Transform[] spawnPoints; //

    public List<List<Vector3>> posRotations = new List<List<Vector3>>
    {
        new List<Vector3> { new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0, -1, 0) },
        new List<Vector3> { new Vector3(0.0f, 0.0f, 45.0f), new Vector3(1, -1, 0) },
        new List<Vector3> { new Vector3(0.0f, 0.0f, 90.0f), new Vector3(1, 0, 0) },
        new List<Vector3> { new Vector3(0.0f, 0.0f, 135.0f), new Vector3(1, 1, 0) },
        new List<Vector3> { new Vector3(0.0f, 0.0f, 180.0f), new Vector3(0, 1, 0) },
        new List<Vector3> { new Vector3(0.0f, 0.0f, 225.0f), new Vector3(-1, 1, 0) },
        new List<Vector3> { new Vector3(0.0f, 0.0f, 270.0f), new Vector3(-1, 0, 0) },
        new List<Vector3> { new Vector3(0.0f, 0.0f, 315.0f), new Vector3(-1, -1, 0) },
    };

    public List<Vector3> rotateInfo;
    public bool isRandom;
    public TextAsset beatmapFile;

    [Header("Music")]
    public AudioSource musicSource;
    public AudioClip[] songClips;

    public float bpm;
    public float subBeats;
    public float restartDelay = 2f;
    public TMP_Text statusMessage;
    private List<string> beatmapInfo;
    private readonly List<GameObject> spawnedHits = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(StartSongAndSpawning());
    }

    public void RestartWithBeatmap(TextAsset selectedBeatmap)
    {
        beatmapFile = selectedBeatmap;
        ReadBeatmapInfo();

        if (!isRandom && beatmapInfo.Count <= 2)
        {
            statusMessage.gameObject.SetActive(true);
            statusMessage.text = "No beatmap settings found. Switching to random mode.";
            isRandom = true;
        }

        musicSource.Stop();
        StopAllCoroutines();
        ClearSpawnedHits();
        StartCoroutine(RestartAfterDelay());
    }

    public void SetRandomMode(bool randomMode)
    {
        isRandom = randomMode;
        RestartWithBeatmap(beatmapFile);
    }

    private void ReadBeatmapInfo()
    {
        beatmapInfo = beatmapFile.text
            .Split(new[] { '\r', '\n' }, System.StringSplitOptions.None)
            .Select(line => line.Trim())
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .ToList();
    }

    private IEnumerator RestartAfterDelay()
    {
        yield return new WaitForSeconds(restartDelay);
        statusMessage.gameObject.SetActive(false);
        ReadBeatmapInfo();
        PlayBeatmapSong();
        yield return new WaitForSeconds(GetSpawnTime());
        StartCoroutine(SpawnHits());
    }

    private IEnumerator StartSongAndSpawning()
    {
        ReadBeatmapInfo();
        PlayBeatmapSong();
        yield return new WaitForSeconds(GetSpawnTime());
        StartCoroutine(SpawnHits());
    }

    private void ClearSpawnedHits()
    {
        foreach (GameObject spawnedHit in spawnedHits)
        {
            if (spawnedHit != null) Destroy(spawnedHit);
        }

        spawnedHits.Clear();
    }

    private void PlayBeatmapSong()
    {
        if (beatmapFile == null || musicSource == null) return;

        string beatmapName = NormalizeName(beatmapFile.name);
        foreach (AudioClip songClip in songClips)
        {
            if (NormalizeName(songClip.name) == beatmapName)
            {
                musicSource.clip = songClip;
                musicSource.Play();
                return;
            }
        }

        Debug.LogWarning($"No audio clip matches beatmap '{beatmapFile.name}'.");
    }

    private string NormalizeName(string value)
    {
        string normalized = new string(value
            .ToLowerInvariant()
            .Where(character => (character >= 'a' && character <= 'z') || (character >= '0' && character <= '9'))
            .ToArray());

        return normalized.TrimEnd('0', '1', '2', '3', '4', '5', '6', '7', '8', '9');
    }

    private float GetSpawnTime()
    {
        return 60f / (bpm * subBeats);
    }

    private GameObject FindPrefabWithTag(string prefabTag)
    {
        foreach (GameObject prefab in hitPrefabs)
        {
            if (prefab.CompareTag(prefabTag)) return prefab;
        }

        return null;
    }
    
    IEnumerator SpawnHits()
    {
        ReadBeatmapInfo();

        bpm = float.Parse(beatmapInfo[0]);
        subBeats = float.Parse(beatmapInfo[1]);

        float spawnTime = GetSpawnTime();

        while(isRandom)
        {
            GameObject prefab = hitPrefabs[Random.Range(0, hitPrefabs.Length)];
            List<Vector3> rotationInfo = posRotations[Random.Range(0, posRotations.Count)];
            Vector3 hitRotate = rotationInfo[0];
            Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];

            GameObject spawnedHit = Instantiate(prefab, point.position, Quaternion.Euler(hitRotate));
            spawnedHits.Add(spawnedHit);
            MovingHit movingHit = spawnedHit.GetComponent<MovingHit>();
            movingHit.requiredDirection = rotationInfo[1];

            yield return new WaitForSeconds(spawnTime);
        }
        
        while(!isRandom)
        {
            foreach (string line in beatmapInfo.Skip(2))
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    string[] values = line.Split(',');
                    int rotationIndex = int.Parse(values[0].Trim());
                    int spawnPointIndex = int.Parse(values[1].Trim());
                    GameObject prefab = hitPrefabs[Random.Range(0, hitPrefabs.Length)];
                    if (values.Length >= 3)
                    {
                        GameObject taggedPrefab = FindPrefabWithTag(values[2].Trim());
                        if (taggedPrefab == null) continue;
                        prefab = taggedPrefab;
                    }

                    List<Vector3> rotationInfo = posRotations[rotationIndex];
                    Vector3 hitRotate = rotationInfo[0];
                    Transform point = spawnPoints[spawnPointIndex];

                    GameObject spawnedHit = Instantiate(prefab, point.position, Quaternion.Euler(hitRotate));
                    spawnedHits.Add(spawnedHit);
                    MovingHit movingHit = spawnedHit.GetComponent<MovingHit>();
                    movingHit.requiredDirection = rotationInfo[1];
                }

                yield return new WaitForSeconds(spawnTime);
            }
        }
    }
}
