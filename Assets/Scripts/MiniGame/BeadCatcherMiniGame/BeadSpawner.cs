using UnityEngine;

public class BeadSpawner : MonoBehaviour
{
    public GameObject beadPrefab;
    public Transform spawnPoint;
    public Transform canvasTransform;
    public float spawnInterval = 2f;

    [Range(0f, 1f)] public float priorityChance = 0.4f;
    private string targetLetters = "AGRE";

    void Start()
    {
        InvokeRepeating("SpawnBead", 0f, spawnInterval);
    }

    void SpawnBead()
    {
        GameObject newBead = Instantiate(beadPrefab, spawnPoint.position, Quaternion.identity, spawnPoint);

        char selectedLetter;

        if (Random.value < priorityChance)
        {
            selectedLetter = targetLetters[Random.Range(0, targetLetters.Length)];
        }
        else
        {
            selectedLetter = (char)Random.Range('A', 'Z' + 1);
        }

        newBead.GetComponent<BeadController>().SetLetter(selectedLetter);

        newBead.transform.SetAsFirstSibling();
    }
}