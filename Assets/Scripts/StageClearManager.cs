using UnityEngine;

public class StageClearManager : MonoBehaviour
{
    [SerializeField] private StageScreen[] stageScreens;

    private const string StageClearKey = "StageClear_";

    private void OnEnable()
    {
        foreach (StageScreen stageScreen in stageScreens)
        {
            stageScreen.OnStageClearButtonClicked += HandleStageClear;
        }
        ResetAll(); // 테스트용으로 모든 클리어 데이터 초기화 (실제 게임에서는 제거)
    }

    private void OnDisable()
    {
        foreach (StageScreen stageScreen in stageScreens)
        {
            stageScreen.OnStageClearButtonClicked -= HandleStageClear;
        }
    }

    private void HandleStageClear(int stageNumber)
    {
        PlayerPrefs.SetInt(StageClearKey + stageNumber, 1);
        PlayerPrefs.Save();

        Debug.Log($"{stageNumber} 스테이지 클리어 저장 완료");
    }

    public bool IsStageCleared(int stageNumber)
    {
        return PlayerPrefs.GetInt(StageClearKey + stageNumber, 0) == 1;
    }

    public void ResetStageClear(int stageNumber)
    {
        PlayerPrefs.DeleteKey(StageClearKey + stageNumber);
        PlayerPrefs.Save();
    }

    public void ResetAll()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Debug.Log("모든 클리어 데이터 초기화");
    }
}