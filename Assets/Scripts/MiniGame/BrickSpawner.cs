using UnityEngine;
using UnityEngine.UI;

public class BrickSpawner : MonoBehaviour
{
    [Header("Settings")]
    public GameObject brickPrefab;    
    public Transform brickContainer;

    void Start()
    {
        // 게임 시작 시 6x2 벽돌 생성 실행
        SpawnBricks(6, 2);
    }

    public void SpawnBricks(int columns, int rows)
    {
        int totalBricks = columns * rows;

        for (int i = 0; i < totalBricks; i++)
        {
            GameObject newBrick = Instantiate(brickPrefab, brickContainer);
        }

        GridLayoutGroup grid = brickContainer.GetComponent<GridLayoutGroup>();

        if (grid != null)
        {
            // 강제로 캔버스 업데이트해서 Grid Layout Group으로 정렬 후에 비활성화
            Canvas.ForceUpdateCanvases();
            grid.enabled = false;
        }
    }
}