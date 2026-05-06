using UnityEngine;

public class BeadSpawner : MonoBehaviour
{
    public GameObject beadPrefab;
    public Transform spawnPoint;
    public Transform canvasTransform;
    public float spawnInterval = 2f;

    void Start()
    {
        InvokeRepeating("SpawnBead", 0f, spawnInterval);
    }

    void SpawnBead()
    {
        GameObject newBead = Instantiate(beadPrefab, spawnPoint.position, Quaternion.identity, canvasTransform);

        char randomLetter = (char)Random.Range('A', 'Z' + 1);
        newBead.GetComponent<BeadController>().SetLetter(randomLetter);

        // Hierarchy 최상단으로 이동
        newBead.transform.SetAsFirstSibling();
    }
}