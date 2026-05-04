using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CarLauncher : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("UI References")]
    public Slider powerSlider;
    public Transform gameScreen; 

    [Header("Object References")]
    public GameObject carPrefab;
    public Transform spawnPoint;

    [Header("Settings")]
    public float chargeSpeed = 0.8f;
    public float launchForce = 20f;
    public float spawnDelay = 0.5f;

    private bool isCharging = false;
    private GameObject currentCar;
    private bool isSpawnRoutineRunning = false;

    void Start()
    {
        StartCoroutine(SpawnCarRoutine());
    }

    void Update()
    {
        // 게이지 충전 로직
        if (isCharging && currentCar != null)
        {
            powerSlider.value += Time.deltaTime * chargeSpeed;
            if (powerSlider.value >= 1f) powerSlider.value = 0f;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (currentCar != null) isCharging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isCharging && currentCar != null)
        {
            isCharging = false;
            LaunchCar();
        }
    }

    void LaunchCar()
    {
        float currentPower = powerSlider.value;
        powerSlider.value = 0f;

        // 자동차에 붙어있는 컨트롤러에게 발사 명령 전달
        CarController controller = currentCar.GetComponent<CarController>();
        if (controller != null)
        {
            // 발사 시 Launcher 정보를 넘겨주어 나중에 리스폰을 요청할 수 있게 함
            controller.Launch(transform.right, launchForce * (currentPower + 0.2f), this);
        }

        currentCar = null;
    }

    // 자동차가 없어졌을 때 다시 생성하기 위한 공용 메서드
    public void RequestRespawn()
    {
        if (!isSpawnRoutineRunning)
        {
            StartCoroutine(SpawnCarRoutine());
        }
    }

    IEnumerator SpawnCarRoutine()
    {
        isSpawnRoutineRunning = true;
        yield return new WaitForSeconds(spawnDelay);

        // 생성 시점에 부모(gameScreen)를 바로 지정
        currentCar = Instantiate(carPrefab, gameScreen);

        // 위치를 스폰 포인트로 이동시키고 Z축 초기화
        currentCar.transform.position = spawnPoint.position;
        Vector3 localPos = currentCar.transform.localPosition;
        localPos.z = 0f;
        currentCar.transform.localPosition = localPos;

        // 부모의 스케일에 영향을 받더라도 크기를 1로 고정
        currentCar.transform.localScale = Vector3.one;

        Rigidbody2D rb = currentCar.GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false;

        isSpawnRoutineRunning = false;
    }
}