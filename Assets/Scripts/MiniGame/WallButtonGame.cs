using UnityEngine;
using UnityEngine.UI;

public class WallButtonGame : MonoBehaviour
{
    [Header("버튼 프리팹")]
    public GameObject agreePrefab;    // 동의2 프리팹 (거절과 같은 색, 클릭 시 성공)
    public GameObject disagreePrefab; // 거절 프리팹 (클릭 시 실패)

    [Header("그리드 설정")]
    public int columns = 14;
    public int rows    = 10;
    public Vector2 cellSize    = new(80f, 50f);
    public Vector2 cellSpacing = new(4f, 4f);


    void Awake()
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);
    }

    public void StartMiniGame()
    {
        if (agreePrefab == null || disagreePrefab == null)
        {
            Debug.LogError($"[WallButtonGame] 프리팹 미연결 on {gameObject.name}");
            return;
        }

        if (TryGetComponent<VerticalLayoutGroup>(out var vl)) vl.enabled = false;

        var grid = new GameObject("Grid", typeof(RectTransform));
        grid.transform.SetParent(transform, false);

        var gridRect     = (RectTransform)grid.transform;
        gridRect.anchorMin = Vector2.zero;
        gridRect.anchorMax = Vector2.one;
        gridRect.offsetMin = Vector2.zero;
        gridRect.offsetMax = Vector2.zero;

        var layout               = grid.AddComponent<GridLayoutGroup>();
        layout.cellSize          = cellSize;
        layout.spacing           = cellSpacing;
        layout.constraint        = GridLayoutGroup.Constraint.FixedColumnCount;
        layout.constraintCount   = columns;
        layout.childAlignment    = TextAnchor.MiddleCenter;

        int total     = columns * rows;
        int agreeIdx  = Random.Range(0, total);

        for (int i = 0; i < total; i++)
        {
            bool isAgree = i == agreeIdx;
            var  prefab  = isAgree ? agreePrefab : disagreePrefab;

            var go = Instantiate(prefab, grid.transform);
            if (go.TryGetComponent<ButtonChange>(out var bc)) bc.enabled = false;

            go.SetActive(true);

            var btn = go.GetComponent<Button>();
            btn.onClick = new Button.ButtonClickedEvent();

            if (isAgree)
            {
                btn.onClick.AddListener(MiniGameManager.NotifySuccess);
            }
            else
            {
                btn.onClick.AddListener(MiniGameManager.NotifyFail);
            }
        }
    }
}
