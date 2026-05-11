using System.Collections;
using UnityEngine;

public class LaserEnemy : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform player;

    [Header("타이밍 설정")]
    public float warningTime = 0.5f;
    public float lockBeforeFire = 0.5f;
    public float laserDuration = 0.5f;

    [Header("비주얼 설정")]
    public float blinkSpeed = 15f; // 예고 시 깜빡임 속도

    public void StartLaserAttack()
    {
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(LaserRoutine());
        }
    }

    IEnumerator LaserRoutine()
    {
        lineRenderer.enabled = true;
        Vector3 direction = (player.position - transform.position).normalized;
        float t = 0;

        // 얇은 선 + 깜빡임 + 추적
        while (t < warningTime)
        {
            // 발사 0.5초 전 조준 고정
            if (t < (warningTime - lockBeforeFire))
            {
                direction = (player.position - transform.position).normalized;
            }

            // 얇은 선으로 예고
            lineRenderer.startWidth = lineRenderer.endWidth = 0.05f;

            // 깜빡임 효과 (활성화/비활성화 반복)
            lineRenderer.enabled = (Mathf.FloorToInt(t * blinkSpeed) % 2 == 0);

            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + direction * 30f);

            t += Time.deltaTime;
            yield return null;
        }

        //굵은 실선 (깜빡임 중지)
        lineRenderer.enabled = true;
        lineRenderer.startWidth = lineRenderer.endWidth = 0.6f;

        // 충돌 판정
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 30f);
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            DragController playerCtrl = hit.collider.GetComponent<DragController>();
            if (playerCtrl != null)
            {
                playerCtrl.GetHit();
            }
        }

        yield return new WaitForSeconds(laserDuration);
        lineRenderer.enabled = false;
    }
}
