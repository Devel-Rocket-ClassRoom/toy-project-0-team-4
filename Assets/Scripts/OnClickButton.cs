using UnityEngine;

public class OnClickButton : MonoBehaviour
{
    [Header("화면")]
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private GameObject mainScreen;

    [Header("미니게임 생성 매니저")]
    [SerializeField] private MiniGameSpawner miniGameSpawner;

    public void OnClickStart()
    {
        titleScreen.SetActive(false);
        mainScreen.SetActive(true);

        MainScreenUI mainScreenUI = mainScreen.GetComponent<MainScreenUI>();

        if (mainScreenUI != null)
        {
            mainScreenUI.RefreshStageButtons();
        }
    }

    public void OnClickToTitle()
    {
        if (miniGameSpawner != null)
        {
            miniGameSpawner.DestroyCurrentMiniGame();
        }

        titleScreen.SetActive(true);
        mainScreen.SetActive(false);
    }

    public void OnClickToStage(int stageNumber)
    {
        if (miniGameSpawner == null)
        {
            Debug.LogWarning("MiniGameSpawner가 연결되지 않았습니다.");
            return;
        }

        miniGameSpawner.StartStage(stageNumber);
    }

    public void OnClickCancle()
    {
        if (miniGameSpawner != null)
        {
            miniGameSpawner.DestroyCurrentMiniGame();
        }

        titleScreen.SetActive(true);
        mainScreen.SetActive(false);
    }
}