using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MiniGamePool
{
    [Serializable]
    public class StageMiniGameGroup
    {
        [Header("스테이지 번호")]
        public int stageNumber;

        [Header("해당 스테이지에서 랜덤으로 나올 미니게임 목록")]
        public StageScreen[] miniGamePrefabs;

        [NonSerialized]
        public readonly List<int> availableIndexes = new List<int>();
    }

    [Header("스테이지별 미니게임 목록")]
    [SerializeField]
    private StageMiniGameGroup[] stageGroups;

    public bool HasStage(int stageNumber)
    {
        StageMiniGameGroup group = FindGroup(stageNumber);

        return group != null && group.miniGamePrefabs != null && group.miniGamePrefabs.Length > 0;
    }

    public void ResetAllPools()
    {
        if (stageGroups == null)
        {
            return;
        }

        foreach (StageMiniGameGroup group in stageGroups)
        {
            ResetPool(group);
        }

        Debug.Log("모든 스테이지 미니게임 랜덤 풀 초기화");
    }

    public StageScreen GetRandomPrefabByStage(int stageNumber)
    {
        StageMiniGameGroup group = FindGroup(stageNumber);

        if (group == null)
        {
            Debug.LogWarning($"{stageNumber} 스테이지 그룹을 찾지 못했습니다.");
            return null;
        }

        if (group.miniGamePrefabs == null || group.miniGamePrefabs.Length == 0)
        {
            Debug.LogWarning($"{stageNumber} 스테이지에 미니게임 Prefab이 없습니다.");
            return null;
        }

        if (group.availableIndexes.Count == 0)
        {
            ResetPool(group);
        }

        if (group.availableIndexes.Count == 0)
        {
            return null;
        }

        int randomListIndex = UnityEngine.Random.Range(0, group.availableIndexes.Count);
        int prefabIndex = group.availableIndexes[randomListIndex];

        group.availableIndexes.RemoveAt(randomListIndex);

        return group.miniGamePrefabs[prefabIndex];
    }

    private StageMiniGameGroup FindGroup(int stageNumber)
    {
        if (stageGroups == null)
        {
            return null;
        }

        foreach (StageMiniGameGroup group in stageGroups)
        {
            if (group == null)
            {
                continue;
            }

            if (group.stageNumber == stageNumber)
            {
                return group;
            }
        }

        return null;
    }

    private void ResetPool(StageMiniGameGroup group)
    {
        if (group == null)
        {
            return;
        }

        group.availableIndexes.Clear();

        if (group.miniGamePrefabs == null)
        {
            return;
        }

        for (int i = 0; i < group.miniGamePrefabs.Length; i++)
        {
            if (group.miniGamePrefabs[i] != null)
            {
                group.availableIndexes.Add(i);
            }
        }
    }
}
