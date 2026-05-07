using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 미니게임 중복 방지 랜덤 선택
/// </summary>

[System.Serializable]
public class StageGroup
{
    public string stageName; // 인스펙터 식별용 (예: Stage 1)
    public StageScreen[] miniGamePrefabs;

    // 해당 스테이지에서 아직 사용하지 않은 인덱스들
    [HideInInspector] public List<int> availableIndexes = new List<int>();
}

[System.Serializable]
public class MiniGamePool
{
    [Header("스테이지별 미니게임 설정")]
    [SerializeField] private List<StageGroup> stageGroups = new List<StageGroup>();

    // 모든 스테이지의 풀을 초기화
    public void ResetAllPools()
    {
        for (int i = 0; i < stageGroups.Count; i++)
        {
            ResetStagePool(i);
        }
    }

    // 특정 스테이지의 풀만 초기화
    public void ResetStagePool(int stageIndex)
    {
        if (stageIndex < 0 || stageIndex >= stageGroups.Count) return;

        StageGroup group = stageGroups[stageIndex];
        group.availableIndexes.Clear();

        if (group.miniGamePrefabs == null) return;

        for (int i = 0; i < group.miniGamePrefabs.Length; i++)
        {
            if (group.miniGamePrefabs[i] != null)
            {
                group.availableIndexes.Add(i);
            }
        }
    }

    // 특정 스테이지 번호에 맞는 랜덤 프리팹 반환
    public StageScreen GetRandomPrefabByStage(int stageNumber)
    {
        // 리스트 인덱스는 0부터 시작하므로 stageNumber - 1
        int index = stageNumber - 1;

        if (index < 0 || index >= stageGroups.Count)
        {
            Debug.LogError($"{stageNumber}번에 해당하는 스테이지 설정이 StageGroups에 없습니다!");
            return null;
        }

        StageGroup group = stageGroups[index];

        // 해당 스테이지 미니게임을 다 썼다면 다시 채움
        if (group.availableIndexes.Count == 0)
        {
            ResetStagePool(index);
        }

        if (group.availableIndexes.Count == 0) return null;

        int randomListIndex = Random.Range(0, group.availableIndexes.Count);
        int prefabIndex = group.availableIndexes[randomListIndex];

        group.availableIndexes.RemoveAt(randomListIndex);

        return group.miniGamePrefabs[prefabIndex];
    }

    public bool HasStage(int stageNumber)
    {
        return stageGroups.Count >= stageNumber && stageGroups[stageNumber - 1].miniGamePrefabs.Length > 0;
    }
}