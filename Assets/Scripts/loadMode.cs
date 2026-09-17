using UnityEngine;

public class loadMode : MonoBehaviour
{
    public HitSpawner hitSpawner;
    public GameObject[] modeCubes;
    public Material activeBlockMaterial;
    public Material inactiveBlockMaterial;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("sword")) return;

        bool modeSelected = false;
        if (CompareTag("isRandom"))
        {
            hitSpawner.SetRandomMode(true);
            modeSelected = true;
        }
        else if (CompareTag("isNotRandom"))
        {
            hitSpawner.SetRandomMode(false);
            modeSelected = true;
        }

        if (modeSelected) UpdateModeColors();
    }

    private void UpdateModeColors()
    {
        foreach (GameObject mode in modeCubes)
        {
            Renderer modeRenderer = mode.GetComponent<Renderer>();
            modeRenderer.sharedMaterial = mode == gameObject
                ? activeBlockMaterial
                : inactiveBlockMaterial;
        }
    }
}
