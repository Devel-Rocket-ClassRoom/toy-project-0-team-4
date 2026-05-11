using System.Collections;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float rotationSpeed = 150f;
    public float fireRate = 0.15f;
    private float nextFireTime;

    public Vector3 originalPosition;
    public Quaternion originalRotation;

    void Awake()
    {
        this.enabled = false;
    }

    void OnEnable()
    {
        nextFireTime = Time.time + 0.5f;
    }

    void Update()
    {
        // 적 버튼 회전
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        // 탄막 발사
        if (Time.time >= nextFireTime)
        {
            Instantiate(bulletPrefab, transform.position, transform.rotation, transform.parent);
            nextFireTime = Time.time + fireRate;
        }
    }

    public void FlyAway()
    {
        StopAllCoroutines();
        StartCoroutine(FlyAwayRoutine());
    }

    IEnumerator FlyAwayRoutine()
    {
        this.enabled = false;
        Vector3 exitDirection = (transform.localPosition).normalized; // 중심에서 바깥 방향
        float t = 0;
        while (t < 1.0f)
        {
            transform.localPosition += exitDirection * 1500f * Time.deltaTime; // 밖으로 튕겨나감
            t += Time.deltaTime;
            yield return null;
        }
        gameObject.SetActive(false);
    }

    public void FlyIn(Vector3 targetPos)
    {
        // 1. 코루틴을 시작하기 전에 오브젝트를 먼저 활성화합니다. (매우 중요)
        gameObject.SetActive(true);

        // 2. 이제 오브젝트가 활성화되었으므로 코루틴을 시작할 수 있습니다.
        StopAllCoroutines();
        StartCoroutine(FlyInRoutine(targetPos));
    }

    IEnumerator FlyInRoutine(Vector3 targetPos)
    {
        // 진입 중에는 탄막이 나가지 않도록 스크립트만 꺼둡니다.
        this.enabled = false;

        // 시작 위치 설정: 목표 지점 방향의 화면 바깥쪽
        Vector3 direction = targetPos.normalized;
        if (direction == Vector3.zero)
            direction = Vector3.up;
        transform.localPosition = targetPos + (direction * 1000f);

        // 3. 목표 위치로 이동 (0.5초 동안 빠르게 진입)
        float t = 0;
        Vector3 startPos = transform.localPosition;
        while (t < 1.0f)
        {
            t += Time.deltaTime * 2.0f;
            transform.localPosition = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        transform.localPosition = targetPos;
    }
}
