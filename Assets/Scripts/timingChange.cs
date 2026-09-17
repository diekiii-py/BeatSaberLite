using UnityEngine;

public class timingChange : MonoBehaviour
{
    public GameObject[] modeCubes;
    public HitSpawner hitSpawner;
    public Material activeBlockMaterial;
    private Color originalColor;

    private void Start()
    {
        originalColor = GetComponent<Renderer>().material.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("sword")) return;

        if (CompareTag("timingEarly"))
        {
            hitSpawner.transform.position += new Vector3(0, 0, -0.5f);
        }
        else if (CompareTag("timingLate"))
        {
            hitSpawner.transform.position += new Vector3(0, 0, 0.5f);
        }

        StartCoroutine(ResetColor());
    }

    private System.Collections.IEnumerator ResetColor()
    {
        Renderer blockRenderer = GetComponent<Renderer>();
        blockRenderer.material.color = activeBlockMaterial.color;
        yield return new WaitForSeconds(0.5f);
        blockRenderer.material.color = originalColor;
    }
}
