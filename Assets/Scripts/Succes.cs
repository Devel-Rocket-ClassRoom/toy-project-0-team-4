using UnityEngine;

public class Succes : MonoBehaviour
{
    [Header("오디오 매니저")]
    [SerializeField]
    private AudioManager audioManager;

    [Header("성공 효과음 인덱스")]
    [SerializeField]
    private int successSfxIndex = 4;

    private void Awake()
    {
        // Prefab 안에서는 AudioManager 연결이 비어있을 수 있으므로 자동 탐색
        if (audioManager == null)
        {
            audioManager = FindFirstObjectByType<AudioManager>();
        }
    }

    private void OnEnable()
    {
        // AudioManager가 없으면 효과음만 건너뜀
        if (audioManager != null)
        {
            audioManager.PlaySfx(successSfxIndex);
        }
        else
        {
            Debug.LogWarning("Succes: AudioManager가 연결되지 않았습니다.");
        }
    }
}
