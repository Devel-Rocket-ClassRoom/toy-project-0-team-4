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
        Debug.Log($"{group.stageName} 풀 초기화 완료 / 개수: {group.availableIndexes.Count}");
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
            Debug.Log($"{group.stageName}의 모든 게임을 소진하여 풀을 재설정합니다.");
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


//[System.Serializable]
//public class MiniGamePool
//{
//    [Header("미니게임 프리팹 목록")]
//    [SerializeField] private StageScreen[] miniGamePrefabs;

//    // 아직 나오지 않은 미니게임 인덱스 목록
//    private readonly List<int> availableMiniGameIndexes = new List<int>();

//    public bool HasPrefab()
//    {
//        return miniGamePrefabs != null && miniGamePrefabs.Length > 0;
//    }

//    public void ResetPool()
//    {
//        availableMiniGameIndexes.Clear();

//        if (miniGamePrefabs == null)
//        {
//            return;
//        }

//        for (int i = 0; i < miniGamePrefabs.Length; i++)
//        {
//            if (miniGamePrefabs[i] != null)
//            {
//                availableMiniGameIndexes.Add(i);
//            }
//        }

//        Debug.Log($"미니게임 랜덤 목록 초기화 / 개수: {availableMiniGameIndexes.Count}");
//    }

//    public StageScreen GetRandomPrefabWithoutDuplicate()
//    {
//        // 모든 미니게임이 한 번씩 나왔다면 다시 초기화
//        if (availableMiniGameIndexes.Count == 0)
//        {
//            Debug.Log("모든 미니게임이 한 번씩 나왔습니다. 미니게임 목록을 다시 초기화합니다.");
//            ResetPool();
//        }

//        if (availableMiniGameIndexes.Count == 0)
//        {
//            return null;
//        }

//        int randomListIndex = Random.Range(0, availableMiniGameIndexes.Count);
//        int prefabIndex = availableMiniGameIndexes[randomListIndex];

//        // 뽑힌 미니게임은 후보 목록에서 제거해서 중복 방지
//        availableMiniGameIndexes.RemoveAt(randomListIndex);

//        return miniGamePrefabs[prefabIndex];
//    }
//}