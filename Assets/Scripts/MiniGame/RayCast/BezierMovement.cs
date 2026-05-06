using UnityEngine;

public class BezierMovement : MonoBehaviour
{
    [Header("Area Settings")]
    public RectTransform moveArea; // 이동을 제한할 패널의 RectTransform
    public float padding = 50f;    // 패널 테두리에서 얼마나 떨어져 있을지

    [Header("Movement Settings")]
    private float duration;
    private float timer = 0f;

    private Vector3 p0, p1, p2;
    private RectTransform myRect;

    void Start()
    {
        duration = Random.Range(1.0f, 2.0f);
        myRect = GetComponent<RectTransform>();

        // 만약 moveArea를 할당하지 않았다면 부모를 기본 영역으로 설정
        if (moveArea == null)
            moveArea = transform.parent.GetComponent<RectTransform>();

        SetNewPath();
    }

    void Update()
    {
        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / duration);

        // 2차 베지어 곡선 공식: B(t) = (1-t)^2P0 + 2(1-t)tP1 + t^2P2
        transform.localPosition = CalculateBezier(t, p0, p1, p2);

        if (t >= 1.0f)
        {
            SetNewPath();
        }
    }

    void SetNewPath()
    {
        timer = 0f;
        p0 = transform.localPosition; // 현재 위치에서 시작

        // 패널 크기 안에서 랜덤한 제어점과 도착점 생성
        p1 = GetRandomPointInPanel();
        p2 = GetRandomPointInPanel();
    }

    Vector3 GetRandomPointInPanel()
    {
        // moveArea의 크기를 가져옴
        float width = moveArea.rect.width / 2f - padding;
        float height = moveArea.rect.height / 2f - padding;

        float x = Random.Range(-width, width);
        float y = Random.Range(-height, height);

        return new Vector3(x, y, 0);
    }

    Vector3 CalculateBezier(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        return u * u * p0 + 2 * u * t * p1 + t * t * p2;
    }
}