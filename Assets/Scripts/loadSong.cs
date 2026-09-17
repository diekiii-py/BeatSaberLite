using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class loadSong : MonoBehaviour
{
    [Header("Songs")]
    public GameObject[] songs;
    public TextAsset[] beatmaps;
    public HitSpawner hitSpawner;

        
    [Header("Select colors")]
    public Material activeblockcolor;
    public Material inactiveblockcolor;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("sword")) return;

        TMP_Text text = GetComponentInChildren<TMP_Text>();
        if (text == null) return;

        string songName = text.text.Trim();
        TextAsset beatmapFile = FindBeatmap(songName);
        if (beatmapFile == null)
        {
            Debug.LogWarning($"No beatmap found for '{songName}'.");
            return;
        }

        foreach (GameObject song in songs)
        {
            if (song == gameObject)
            {
                Renderer renderer = song.GetComponent<Renderer>();
                renderer.sharedMaterial = activeblockcolor;
            }
            else
            {
                Renderer renderer = song.GetComponent<Renderer>();
                renderer.sharedMaterial = inactiveblockcolor;
            }
        }
        
        hitSpawner.RestartWithBeatmap(beatmapFile);

    }

    private TextAsset FindBeatmap(string songName)
    {
        foreach (TextAsset beatmap in beatmaps)
        {
            if (string.Equals(beatmap.name, songName, System.StringComparison.OrdinalIgnoreCase))
                return beatmap;
        }

        return Resources.Load<TextAsset>(songName);
    }

}
