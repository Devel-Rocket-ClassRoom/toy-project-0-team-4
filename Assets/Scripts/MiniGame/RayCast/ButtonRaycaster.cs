using UnityEngine;
using UnityEngine.UI;

public class ButtonRaycaster : MonoBehaviour
{
    [Header("Raycast Settings")]
    public Transform rayOrigin;
    public float maxDistance = 4.5f;
    public LayerMask targetLayer;

    [Header("State")]
    public bool isAgree = false; // ���� ���� �������� ����

    [Header("UI References")]
    public LineRenderer lineRenderer;
    public Image buttonImage;
    public Sprite agreeSprite;
    public Sprite disagreeSprite;
    public StageScreen stageScreen;

    void Update()
    {
        // 1. ����ĳ��Ʈ �߻�
        RaycastHit2D hit = Physics2D.Raycast(
            rayOrigin.position,
            Vector2.up,
            maxDistance,
            targetLayer
        );

        // 2. LineRenderer ������
        lineRenderer.SetPosition(0, Vector3.zero);

        if (hit.collider != null)
        {
            // �浹 ���� ��� �� �� �׸���
            Vector3 localHitPoint = rayOrigin.InverseTransformPoint(hit.point);
            lineRenderer.SetPosition(1, localHitPoint);

            // �±׿� ���� ���� ������Ʈ
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
