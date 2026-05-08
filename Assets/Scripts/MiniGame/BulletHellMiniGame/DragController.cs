using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class DragController : MonoBehaviour, IPointerClickHandler
{
    private bool isFollowing = false;
    private bool gameStarted = false;

    private RectTransform rectTransform;
    private RectTransform parentRect; 
    private Canvas canvas;

    public PatternDirector patternDirector; 
    public SliderTimer sliderTimer; 
    public StageScreen stageScreen;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        parentRect = rectTransform.parent as RectTransform; // 부모 RectTransform 참조
    }

    public void OnPointerClick(PointerEventData eventData) 
    {
        // 1. 아직 게임이 시작되지 않았을 때만 실행
        if (!gameStarted)
        {
            gameStarted = true; // 시작됨으로 변경 (이제 다시 클릭해도 이 블록은 실행되지 않음)
            isFollowing = true;

            if (patternDirector != null)
            {
                patternDirector.StartGameSequence(); // 시퀀스 시작
            }

            if (sliderTimer != null)
            {
                sliderTimer.StartTimer();
            }
        }
    }

    void Update()
    {
        if (isFollowing) 
        {
            // 스크린 좌표를 UI 좌표로 변환
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform.parent as RectTransform,
                Input.mousePosition,
                canvas.worldCamera,
                out localPoint
            );

            // --- 위치 제한(Clamping) 로직 추가 ---
            // 1. 부모 영역의 절반 크기 계산
            float minX = parentRect.rect.xMin + (rectTransform.rect.width / 2);
            float maxX = parentRect.rect.xMax - (rectTransform.rect.width / 2);
            float minY = parentRect.rect.yMin + (rectTransform.rect.height / 2);
            float maxY = parentRect.rect.yMax - (rectTransform.rect.height / 2);

            // 2. 계산된 범위 내로 좌표 고정
            float clampedX = Mathf.Clamp(localPoint.x, minX, maxX);
            float clampedY = Mathf.Clamp(localPoint.y, minY, maxY);

            // 3. 보정된 좌표 적용
            rectTransform.localPosition = new Vector2(clampedX, clampedY);
        }
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Bullet") && isFollowing)
        {
            GetHit();
        }
    }

    public void GetHit()
    {
        if (!isFollowing) return; // 이미 피격 중이면 무시

        isFollowing = false;

        // 1. 타이머 정지
        if (sliderTimer != null)
        {
            sliderTimer.StopTimer();
        }

        if (patternDirector != null)
        {
            patternDirector.StopGameSequence();
        }

        // 2. 튕겨나가는 연출 시작
        StartCoroutine(PlayerFlyAwayRoutine());
    }

    IEnumerator PlayerFlyAwayRoutine()
    {
        // 중심에서 현재 위치 방향으로 튕겨나감
        Vector3 exitDirection = (transform.localPosition).normalized;
        if (exitDirection == Vector3.zero) exitDirection = Vector3.down;

        float t = 0;
        while (t < 1.0f)
        {
            transform.localPosition += exitDirection * 1200f * Time.deltaTime;
            t += Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false); // 버튼 사라짐

        stageScreen.GameOver(); 
    }

    public void StopFollowing()
    {
        isFollowing = false; // 마우스 추적 변수를 꺼버림
    }
}