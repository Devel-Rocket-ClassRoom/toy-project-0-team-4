using UnityEngine;

public partial class MiniGameSpawner
{
    public void DestroyCurrentMiniGame()
    {
        if (currentMiniGame == null)
        {
            return;
        }

        currentMiniGame.OnStageClearButtonClicked -= HandleStageClear;
        currentMiniGame.OnGameOver -= HandleGameOver;

        Destroy(currentMiniGame.gameObject);
        currentMiniGame = null;
    }

    private void DestroyOTPInstance()
    {
        if (otpInstance == null)
        {
            return;
        }

        Destroy(otpInstance.gameObject);
        otpInstance = null;
    }

    private void ApplyStageTitleToCurrentMiniGame(int stageNumber)
    {
        if (currentMiniGame == null)
        {
            return;
        }

        if (mainScreenUI == null)
        {
            Debug.LogWarning("MainScreenUI가 MiniGameSpawner에 연결되지 않았습니다.");
            return;
        }

        string stageTitle = mainScreenUI.GetStageTitle(stageNumber);

        MiniGameTitleText[] titleTexts = currentMiniGame.GetComponentsInChildren<MiniGameTitleText>(
            true
        );

        if (titleTexts == null || titleTexts.Length == 0)
        {
            Debug.LogWarning($"{currentMiniGame.name} 안에서 MiniGameTitleText를 찾지 못했습니다.");
            return;
        }

        foreach (MiniGameTitleText titleText in titleTexts)
        {
            titleText.SetTitle(stageTitle);
        }

        Debug.Log($"{stageNumber} 스테이지 제목 적용: {stageTitle}");
    }
}
