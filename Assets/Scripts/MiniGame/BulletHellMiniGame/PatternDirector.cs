using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PatternDirector : MonoBehaviour
{
    public List<BulletEnemy> enemies;
    public Transform player;
    public RectTransform canvasRect;

    public void StartGameSequence()
    {
        StartCoroutine(GameRoutine());
    }

    IEnumerator GameRoutine()
    {
        // 0. 초기화: 모든 적을 일단 비활성화 상태로 시작
        foreach (var e in enemies)
        {
            e.gameObject.SetActive(false);
            e.enabled = false;
        }
        yield return new WaitForSeconds(0.5f);

        // ==========================================
        // 패턴 1: 인덱스 0, 1번 버튼 진입 및 탄막 공격
        // ==========================================
        enemies[0].FlyIn(enemies[0].originalPosition);
        enemies[1].FlyIn(enemies[1].originalPosition);

        yield return new WaitForSeconds(0.6f); // 진입 완료 대기

        enemies[0].enabled = true; // OnEnable에 의해 0.5초 뒤 발사 시작
        enemies[1].enabled = true;

        yield return new WaitForSeconds(5.0f); // 5초간 공격

        enemies[0].FlyAway(); // 밖으로 튕겨나가며 비활성화
        enemies[1].FlyAway();
        yield return new WaitForSeconds(1.0f); // 퇴장 대기


        // ==========================================
        // 패턴 2: 랜덤 위치에서 레이저 공격 (4회)
        // ==========================================
        for (int i = 0; i < 4; i++)
        {
            // 랜덤하게 적 하나 선택
            BulletEnemy e = enemies[Random.Range(0, enemies.Count)];

            // 캔버스 내 랜덤 목적지 계산
            float randomX = Random.Range(canvasRect.rect.xMin + 150, canvasRect.rect.xMax - 150);
            float randomY = Random.Range(canvasRect.rect.yMin + 150, canvasRect.rect.yMax - 150);
            Vector3 targetPos = new Vector3(randomX, randomY, 0);

            e.FlyIn(targetPos); // 랜덤 위치로 날아옴
            yield return new WaitForSeconds(0.6f); // 진입 대기

            // 레이저 발사 (탄막 스크립트는 끄고 레이저만 실행)
            LaserEnemy laser = e.GetComponent<LaserEnemy>();
            if (laser != null)
            {
                laser.player = this.player;
                laser.StartLaserAttack();
            }

            yield return new WaitForSeconds(1.0f); // 레이저 예고 및 발사 시간 대기

            e.FlyAway(); // 레이저 쏘고 다시 퇴장
            yield return new WaitForSeconds(0.5f); // 다음 레이저 전 짧은 휴식
        }


        // ==========================================
        // 패턴 3: 모든 적이 원래 자리로 진입하여 총공격
        // ==========================================
        foreach (var e in enemies)
        {
            // 레이저 잔상이 남지 않도록 초기화
            LaserEnemy le = e.GetComponent<LaserEnemy>();
            if (le != null)
            {
                le.StopAllCoroutines();
                le.lineRenderer.enabled = false;
            }

            // 원래 저장된 회전값으로 초기화 (뒤집힘 방지)
            e.transform.localRotation = e.originalRotation;

            // 인스펙터에서 설정한 각자의 원래 위치로 날아옴
            e.FlyIn(e.originalPosition);
        }

        yield return new WaitForSeconds(0.7f); // 모든 적 진입 완료 대기

        // 모든 적 탄막 발사 활성화
        foreach (var e in enemies)
        {
            e.enabled = true; // 0.5초 대기 후 일제히 발사
        }

        yield return new WaitForSeconds(5.0f); // 5초간 지옥의 탄막

        // 모든 공격 종료 및 정지
        foreach (var e in enemies)
        {
            e.enabled = false;
        }

        Debug.Log("모든 패턴 시퀀스 완료!");
    }
}