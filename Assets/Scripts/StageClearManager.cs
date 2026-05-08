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
            return;
        }

        clearedStages.Add(stageNumber);

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

    public bool AllCleared(int totalStages)
    {
        for (int i = 1; i <= totalStages; i++)
            if (!clearedStages.Contains(i)) return false;
        return true;
    }

    public void ResetAll()
    {
        clearedStages.Clear();
    }
}