using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

// IPointerDownHandler, IPointerUpHandler 인터페이스를 사용하면 버튼을 누르고 있는 동안과 뗄 때의 이벤트를 처리할 수 있음.
public class BasketballTest : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("UI References")]
    public Slider powerSlider;
    public RectTransform arrowUI;
    public RectTransform canvasRoot;

    [Header("Object References")]
    public GameObject ballPrefab;
    public Transform spawnPoint;

    [Header("Settings")]
    public float rotationSpeed = 2f;
    public float rotationAngle = 45f;
    public float chargeSpeed = 0.8f;
    public float launchForce = 10f;

    [Header("Animation Settings")]
    public float spawnDelay = 0.5f;    // 발사 후 생성 대기 시간
    public float scaleDuration = 0.3f; // 커지는 데 걸리는 시간

    private bool isCharging = false;
    private bool isSpawnRoutineRunning = false; // 중복 생성 방지
    private GameObject currentBall;

    void Start()
    {
        StartCoroutine(SpawnBallRoutine());
    }

    void Update()
    {
        // 화살표 회전
        float angle = (Mathf.Sin(Time.time * rotationSpeed) * 45f) - 45f;

        arrowUI.localRotation = Quaternion.Euler(0, 0, angle);

        // 게이지 충전 
        if (isCharging && currentBall != null)
        {
            powerSlider.value += Time.deltaTime * chargeSpeed;
            if (powerSlider.value >= 1f) powerSlider.value = 0f;
        }
    }

    // 버튼을 누를 때 (기 모으기 시작)
    public void OnPointerDown(PointerEventData eventData)
    {
        if (currentBall != null)
        {
            isCharging = true;
        }
    }

    // 버튼을 뗄 때 (발사)
    public void OnPointerUp(PointerEventData eventData)
    {
        if (isCharging && currentBall != null)
        {
            isCharging = false;
            LaunchBall();
        }
    }

    // 공 발사 로직
    void LaunchBall()
    {
        float currentPower = powerSlider.value;

        powerSlider.value = 0f;

        // 발사할 때는 화살표의 회전에 영향을 받지 않기 위해 부모를 캔버스로 변경
        currentBall.transform.SetParent(canvasRoot, true);  
        Vector3 currentPos = currentBall.transform.position;
        currentPos.z = 0f;
        currentBall.transform.position = currentPos;

        Rigidbody2D rb = currentBall.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.simulated = true;
            Vector2 shootDirection = arrowUI.up; // up 프로퍼티 : 화살표가 바라보는 방향을 정규화하여 반환
            // 최소 게이지 보정해서 발사
            rb.AddForce(shootDirection * launchForce * (currentPower + 0.2f), ForceMode2D.Impulse);
        }

        Destroy(currentBall, 5f);
        currentBall = null;

        // 공이 없으면 0.5초 뒤 자동 생성 루틴 시작
        if (!isSpawnRoutineRunning)
        {
            StartCoroutine(SpawnBallRoutine());
        }
    }

    // 공 생성 및 스케일 업 애니메이션 코루틴
    IEnumerator SpawnBallRoutine()
    {
        isSpawnRoutineRunning = true;
        yield return new WaitForSeconds(spawnDelay);

        currentBall = Instantiate(ballPrefab);
        currentBall.transform.SetParent(spawnPoint, false); // 로컬 좌표 유지
        currentBall.transform.localPosition = Vector3.zero;

        // 물리 정지
        Rigidbody2D rb = currentBall.GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false;

        // --- 스케일 업 애니메이션 ---
        float timer = 0f;
        currentBall.transform.localScale = Vector3.zero; // 0에서 시작

        while (timer < scaleDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / scaleDuration;

            // 생성되면 scale이 0에서 1로 커짐
            currentBall.transform.localScale = Vector3.one * progress;
            yield return null;
        }
        currentBall.transform.localScale = Vector3.one; // 마지막에 정확히 1로 고정
        // ---------------------------

        isSpawnRoutineRunning = false;
    }
}