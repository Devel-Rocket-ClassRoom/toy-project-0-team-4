using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour
{
    private Rigidbody2D rb;
    private CarLauncher launcher; // 리스폰 요청을 보낼 부모 스크립트 참조
    private bool isLaunched = false;
    private bool isProcessing = false;
    private string currentZoneTag = "";

    [SerializeField] private float stopThreshold = 0.1f;
    [SerializeField] private StageScreen stageScreen;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.centerOfMass = new Vector2(0, -0.5f);
    }

    // Launcher에서 호출하는 발사 함수
    public void Launch(Vector2 direction, float force, CarLauncher launcherRef)
    {
        launcher = launcherRef;
        rb.simulated = true; // 물리 활성화
        rb.AddForce(direction * force, ForceMode2D.Impulse);
        isLaunched = true;
    }

    void Update()
    {
        // 발사 후 멈췄는지 체크
        if (isLaunched && !isProcessing && rb.linearVelocity.magnitude < stopThreshold)
        {
            StartCoroutine(CheckResultRoutine());
        }
    }

    IEnumerator CheckResultRoutine()
    {
        isProcessing = true;
        yield return new WaitForSeconds(0.7f); // 완전히 멈췄는지 잠시 대기

        if (rb.linearVelocity.magnitude < stopThreshold)
        {
            if (currentZoneTag == "ClearZone")
            {
                if (stageScreen == null)
                {
                    stageScreen = GetComponentInParent<StageScreen>(true);
                }

                if (stageScreen != null)
                {
                    stageScreen.ClearStage();
                }
            }
            else if (currentZoneTag == "DeadZone")
            {
                if (stageScreen == null)
                {
                    stageScreen = GetComponentInParent<StageScreen>(true);
                }

                if (stageScreen != null)
                {
                    stageScreen.GameOver();
                }
            }
            else
            {
                launcher.RequestRespawn();
                Destroy(gameObject);
            }
        }
        else
        {
            isProcessing = false; // 다시 움직이면 판정 취소
        }
    }

    // 현재 어떤 트리거 안에 있는지 실시간 기록
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("ClearZone") || collision.CompareTag("DeadZone"))
        {
            currentZoneTag = collision.tag;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(currentZoneTag))
        {
            currentZoneTag = "Untagged";
        }
    }
}