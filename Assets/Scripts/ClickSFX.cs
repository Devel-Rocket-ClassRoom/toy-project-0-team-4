using UnityEngine;

public class ClickSFX : MonoBehaviour
{
    [Header("오디오 매니저")]
    [SerializeField]
    private AudioManager audioManager;

    [Header("클릭 효과음 인덱스")]
    [SerializeField]
    private int clickSfxIndex = 0; // 클릭 효과음 인덱스 (예: 0)

    private void Awake()
    {
        // Prefab 안에서는 AudioManager 연결이 비어있을 수 있으므로 자동 탐색
        if (audioManager == null)
        {
            audioManager = FindFirstObjectByType<AudioManager>();
        }
    }

    public void OnClick()
    {
        // AudioManager가 없으면 효과음만 건너뜀
        if (audioManager != null)
        {
            audioManager.PlaySfx(clickSfxIndex);
        }
        else
        {
            Debug.LogWarning("ClickSFX: AudioManager가 연결되지 않았습니다.");
        }
    }
}
