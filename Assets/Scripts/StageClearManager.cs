using System;
using System.Collections.Generic;
using UnityEngine;

public class StageClearManager : MonoBehaviour
{
    public event Action<int> OnStageCleared;

    private HashSet<int> clearedStages = new HashSet<int>();

    public void ClearStage(int stageNumber)
    {
        if (clearedStages.Contains(stageNumber))
        {
            Debug.Log($"{stageNumber} 스테이지는 이미 클리어됨");
            return;
        }

        clearedStages.Add(stageNumber);

        Debug.Log($"{stageNumber} 스테이지 클리어 저장 완료");

        OnStageCleared?.Invoke(stageNumber);
    }

    public bool IsStageCleared(int stageNumber)
    {
        return clearedStages.Contains(stageNumber);
    }

    public void ResetStageClear(int stageNumber)
    {
        if (clearedStages.Contains(stageNumber))
        {
            clearedStages.Remove(stageNumber);
        }
    }

    public void ResetAll()
    {
        clearedStages.Clear();

        Debug.Log("모든 스테이지 클리어 상태 초기화");
    }
}