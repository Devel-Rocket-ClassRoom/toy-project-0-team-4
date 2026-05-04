using UnityEngine;
using UnityEngine.UI;

public class StageStartButton : MonoBehaviour
{
    [Header("스테이지 번호")]
    [SerializeField] private int stageNumber;

    [Header("미니게임 생성 매니저")]
    [SerializeField] private MiniGameSpawner miniGameSpawner;

    [Header("오디오 매니저")]
    [SerializeField] private AudioManager audioManager;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();

        if (button != null)
        {
            button.onClick.AddListener(OnClickStageStart);
        }
    }

    private void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(OnClickStageStart);
        }
    }

    private void OnClickStageStart()
    {
        Debug.Log($"[StageStartButton] {stageNumber} 스테이지 시작");

        if (audioManager != null)
        {
            audioManager.PlaySfx(0);
        }

        if (miniGameSpawner == null)
        {
            Debug.LogWarning("MiniGameSpawner가 연결되지 않았습니다.");
            return;
        }

        miniGameSpawner.StartStage(stageNumber);
    }
}