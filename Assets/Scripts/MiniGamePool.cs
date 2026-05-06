using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 미니게임 중복 방지 랜덤 선택
/// </summary>

[System.Serializable]
public class MiniGamePool
{
    [Header("미니게임 프리팹 목록")]
    [SerializeField] private StageScreen[] miniGamePrefabs;

    // 아직 나오지 않은 미니게임 인덱스 목록
    private readonly List<int> availableMiniGameIndexes = new List<int>();

    public bool HasPrefab()
    {
        return miniGamePrefabs != null && miniGamePrefabs.Length > 0;
    }

    public void ResetPool()
    {
        availableMiniGameIndexes.Clear();

        if (miniGamePrefabs == null)
        {
            return;
        }

        for (int i = 0; i < miniGamePrefabs.Length; i++)
        {
            if (miniGamePrefabs[i] != null)
            {
                availableMiniGameIndexes.Add(i);
            }
        }

        Debug.Log($"미니게임 랜덤 목록 초기화 / 개수: {availableMiniGameIndexes.Count}");
    }

    public StageScreen GetRandomPrefabWithoutDuplicate()
    {
        // 모든 미니게임이 한 번씩 나왔다면 다시 초기화
        if (availableMiniGameIndexes.Count == 0)
        {
            Debug.Log("모든 미니게임이 한 번씩 나왔습니다. 미니게임 목록을 다시 초기화합니다.");
            ResetPool();
        }

        if (availableMiniGameIndexes.Count == 0)
        {
            return null;
        }

        int randomListIndex = Random.Range(0, availableMiniGameIndexes.Count);
        int prefabIndex = availableMiniGameIndexes[randomListIndex];

        // 뽑힌 미니게임은 후보 목록에서 제거해서 중복 방지
        availableMiniGameIndexes.RemoveAt(randomListIndex);

        return miniGamePrefabs[prefabIndex];
    }
}