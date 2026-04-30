using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BrickSpawner : MonoBehaviour
{
    public GameObject brickPrefab;
    public GameObject agreeButtonPrefab;
    public Transform brickContainer;

    private List<GameObject> spawnedBricks = new List<GameObject>();

    void Start()
    {
        SpawnBricks(6, 2);
    }

    // 벽돌을 columns * rows 형태로 생성
    public void SpawnBricks(int columns, int rows)
    {
        int totalBricks = columns * rows;

        for (int i = 0; i < totalBricks; i++)
        {
            GameObject newBrick = Instantiate(brickPrefab, brickContainer);
            spawnedBricks.Add(newBrick);
        }

        GridLayoutGroup grid = brickContainer.GetComponent<GridLayoutGroup>();

        if (grid != null)
        {
            // 강제로 캔버스 업데이트해서 Grid Layout Group으로 정렬 후에 비활성화
            Canvas.ForceUpdateCanvases();
            grid.enabled = false;
        }

        PlaceButtonAtRandomBrick();
    }

    private void PlaceButtonAtRandomBrick()
    {
        if (spawnedBricks.Count == 0) return;

        int randomIndex = Random.Range(0, spawnedBricks.Count);
        GameObject targetBrick = spawnedBricks[randomIndex];

        GameObject btn = Instantiate(agreeButtonPrefab, brickContainer);

        // 버튼을 계층 구조의 맨 위로 보내서 벽돌 뒤에 숨김
        btn.transform.SetAsFirstSibling();

        btn.transform.localPosition = targetBrick.transform.localPosition;
    }
}