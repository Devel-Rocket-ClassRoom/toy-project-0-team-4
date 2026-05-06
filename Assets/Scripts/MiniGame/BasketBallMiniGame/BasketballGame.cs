using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

// 버튼을 누르고 있는 동안 힘을 모으고, 버튼을 뗄 때 공을 발사하는 농구 미니게임
public class BasketballGame : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("UI References")]
    public Slider powerSlider;
    public RectTransform arrowUI;

    [Header("Object References")]
    public GameObject ballPrefab;

    // 공이 처음 생성될 위치
    public Transform spawnPoint;

    // 발사된 공이 유지될 부모
    // 비워두면 이 BasketballGame 오브젝트 아래에 유지됨
    [SerializeField] private Transform ballParent;

    [Header("Settings")]
    public float rotationSpeed = 2f;
    public float rotationAngle = 45f;
    public float chargeSpeed = 0.8f;
    public float launchForce = 10f;

    [Header("Animation Settings")]
    public float spawnDelay = 0.5f;
    public float scaleDuration = 0.3f;

    private bool isCharging = false;
    private bool isSpawnRoutineRunning = false;

    // 공이 완전히 생성되어 발사 가능한 상태인지 확인
    private bool isBallReady = false;

    private GameObject currentBall;

    private void Start()
    {
        // ballParent가 비어 있으면 이 미니게임 오브젝트 내부에 공을 유지
        if (ballParent == null)
        {
            ballParent = transform;
        }

        StartCoroutine(SpawnBallRoutine());
    }

    private void Update()
    {
        // 화살표 회전
        if (arrowUI != null)
        {
            float angle = (Mathf.Sin(Time.time * rotationSpeed) * 45f) - 45f;
            arrowUI.localRotation = Quaternion.Euler(0, 0, angle);
        }

        // 게이지 충전
        if (isCharging && currentBall != null && isBallReady)
        {
            if (powerSlider != null)
            {
                powerSlider.value += Time.deltaTime * chargeSpeed;

                if (powerSlider.value >= 1f)
                {
                    powerSlider.value = 0f;
                }
            }
        }
    }

    // 버튼을 누를 때 힘 모으기 시작
    public void OnPointerDown(PointerEventData eventData)
    {
        // 공이 완전히 생성된 뒤에만 충전 가능
        if (currentBall != null && isBallReady)
        {
            isCharging = true;
        }
    }

    // 버튼을 뗄 때 공 발사
    public void OnPointerUp(PointerEventData eventData)
    {
        if (isCharging && currentBall != null && isBallReady)
        {
            isCharging = false;
            LaunchBall();
        }
    }

    // 공 발사 로직
    private void LaunchBall()
    {
        if (currentBall == null)
            return;

        if (!isBallReady)
            return;

        float currentPower = 0f;

        if (powerSlider != null)
        {
            currentPower = powerSlider.value;
            powerSlider.value = 0f;
        }

        // 발사 시작 후에는 현재 공을 다시 조작하지 않도록 상태 변경
        isBallReady = false;

        // currentBall을 바로 null로 만들기 전에 로컬 변수로 보관
        GameObject launchedBall = currentBall;
        currentBall = null;

        // 발사된 공도 미니게임 내부 부모에 유지
        // 기존 canvasRoot로 보내면 Canvas 아래로 빠져나가서 팝업 위에 보일 수 있음
        if (ballParent != null)
        {
            launchedBall.transform.SetParent(ballParent, true);
        }

        Vector3 currentPos = launchedBall.transform.position;
        currentPos.z = 0f;
        launchedBall.transform.position = currentPos;

        Rigidbody2D rb = launchedBall.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.simulated = true;

            Vector2 shootDirection = Vector2.up;

            if (arrowUI != null)
            {
                shootDirection = arrowUI.up;
            }

            rb.AddForce(shootDirection * launchForce * (currentPower + 0.2f), ForceMode2D.Impulse);
        }

        Destroy(launchedBall, 5f);

        // 공이 없으면 다음 공 생성
        if (!isSpawnRoutineRunning)
        {
            StartCoroutine(SpawnBallRoutine());
        }
    }

    // 공 생성 및 스케일 업 애니메이션
    private IEnumerator SpawnBallRoutine()
    {
        isSpawnRoutineRunning = true;
        isBallReady = false;

        yield return new WaitForSeconds(spawnDelay);

        if (ballPrefab == null)
        {
            Debug.LogWarning("BasketballGame: ballPrefab이 연결되지 않았습니다.");
            isSpawnRoutineRunning = false;
            yield break;
        }

        if (spawnPoint == null)
        {
            Debug.LogWarning("BasketballGame: spawnPoint가 연결되지 않았습니다.");
            isSpawnRoutineRunning = false;
            yield break;
        }

        // 생성할 때부터 SpawnPoint 아래에 생성
        GameObject spawnedBall = Instantiate(ballPrefab, spawnPoint, false);
        spawnedBall.transform.localPosition = Vector3.zero;

        currentBall = spawnedBall;

        // 물리 정지
        Rigidbody2D rb = spawnedBall.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.simulated = false;
        }

        // 스케일 업 애니메이션
        float timer = 0f;
        spawnedBall.transform.localScale = Vector3.zero;

        while (timer < scaleDuration)
        {
            // 중간에 공이 삭제되었으면 코루틴 종료
            if (spawnedBall == null)
            {
                isSpawnRoutineRunning = false;
                isBallReady = false;
                yield break;
            }

            timer += Time.deltaTime;

            float progress = timer / scaleDuration;
            spawnedBall.transform.localScale = Vector3.one * progress;

            yield return null;
        }

        if (spawnedBall != null)
        {
            spawnedBall.transform.localScale = Vector3.one;
        }

        // 공 생성 완료 후 발사 가능
        isBallReady = true;
        isSpawnRoutineRunning = false;
    }
}