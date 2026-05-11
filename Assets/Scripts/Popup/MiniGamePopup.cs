using UnityEngine;

[System.Serializable]
public partial class MiniGamePopup
{
    [Header("팝업 루트")]
    [SerializeField]
    private GameObject popupRoot;

    [Header("팝업 오브젝트")]
    [SerializeField]
    private GameObject successPopup;

    [SerializeField]
    private GameObject failPopup;

    [SerializeField]
    private GameObject maintenancePopup;

    [SerializeField]
    private GameObject timeoutPopup;

    [SerializeField]
    private GameObject meaningErrorPopup;

    // 현재 성공 / 실패 / 타임아웃 / 의미불명 팝업 중 하나가 떠 있는지 확인
    public bool IsResultOpen { get; private set; }

    // 점검시간 팝업은 최우선 팝업
    public bool IsMaintenanceOpen { get; private set; }

    /// <summary>
    /// 팝업 상태 초기화
    /// </summary>
    public void ResetState()
    {
        IsResultOpen = false;
        IsMaintenanceOpen = false;
    }
}
