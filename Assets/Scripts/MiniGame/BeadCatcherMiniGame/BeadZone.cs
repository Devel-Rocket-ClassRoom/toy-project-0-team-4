using UnityEngine;

public enum ZoneType { RightEnd, Collector }

public class BeadZone : MonoBehaviour
{
    public ZoneType zoneType;
    private BeadGameManager cachedManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BeadController bead = collision.GetComponent<BeadController>();
        if (bead == null) return;

        if (zoneType == ZoneType.Collector)
        {
            // 싱글톤 대신 부모 계층에서 매니저 탐색 (독립 구조)
            if (cachedManager == null)
                cachedManager = GetComponentInParent<BeadGameManager>();

            if (cachedManager != null)
                cachedManager.AddLetter(bead.letter);
        }

        Destroy(collision.gameObject);
    }
}