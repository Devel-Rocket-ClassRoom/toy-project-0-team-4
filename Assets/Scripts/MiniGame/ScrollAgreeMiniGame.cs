using UnityEngine;
using UnityEngine.UI;

public class ScrollAgreeMiniGame : MonoBehaviour
{
    [Header("UI 참조 (Inspector에서 연결)")]
    [SerializeField] private Button agreeButton;
    [SerializeField] private Button disagreeButton;

    public void StartMiniGame()
    {
        agreeButton.onClick.AddListener(() => MiniGameManager.NotifySuccess());
        disagreeButton.onClick.AddListener(() => MiniGameManager.NotifyFail());
    }
}
