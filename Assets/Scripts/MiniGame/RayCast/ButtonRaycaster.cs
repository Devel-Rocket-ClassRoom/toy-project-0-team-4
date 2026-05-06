using UnityEngine;
using UnityEngine.UI;

public class ButtonRaycaster : MonoBehaviour
{
    [Header("Raycast Settings")]
    public Transform rayOrigin;
    public float maxDistance = 4.5f;
    public LayerMask targetLayer;

    [Header("State")]
    public bool isAgree = false; // 현재 동의 상태인지 여부

    [Header("UI References")]
    public LineRenderer lineRenderer;
    public Image buttonImage;
    public Sprite agreeSprite;
    public Sprite disagreeSprite;
    public StageScreen stageScreen;

    void Update()
    {
        // 1. 레이캐스트 발사
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin.position, Vector2.up, maxDistance, targetLayer);

        // 2. LineRenderer 시작점
        lineRenderer.SetPosition(0, Vector3.zero);

        if (hit.collider != null)
        {
            // 충돌 지점 계산 및 선 그리기
            Vector3 localHitPoint = rayOrigin.InverseTransformPoint(hit.point);
            lineRenderer.SetPosition(1, localHitPoint);

            // 태그에 따른 상태 업데이트
            if (hit.collider.CompareTag("Agree"))
            {
                SetButtonState(true);
            }
            else if (hit.collider.CompareTag("Disagree"))
            {
                SetButtonState(false);
            }
        }
    }

    void SetButtonState(bool state)
    {
        isAgree = state;
        buttonImage.sprite = state ? agreeSprite : disagreeSprite;
    }

    public void OnMainButtonClick()
    {
        if (isAgree)
        {
            if (stageScreen != null)
            {
                stageScreen.ClearStage();
            }
        }
        else
        {
            if (stageScreen != null)
            {
                stageScreen.GameOver();
            }
        }
    }
}