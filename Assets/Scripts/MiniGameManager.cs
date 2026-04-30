using UnityEngine;
using System;

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager Instance { get; private set; }

    // 각 미니게임 시작 이벤트
    public static event Action OnGame1Start;
    public static event Action OnGame2Start;
    public static event Action OnGame3Start;
    // 미니게임 종료 이벤트 (성공/실패)
    public static event Action OnMiniGameSuccess;
    public static event Action OnMiniGameFail;

    // 전체 게임 클리어
    public static event Action OnAllGamesCleared;

    private int currentGameIndex = 0;
    private readonly int totalGames = 3;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        OnMiniGameSuccess += HandleSuccess;
        OnMiniGameFail += HandleFail;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int random = UnityEngine.Random.Range(1, 4); // 1, 2, 3 중 랜덤
            Debug.Log($"[Test] 스페이스바 → 미니게임 {random} 호출");
            StartGame(random);
        }
    }

    void OnDestroy()
    {
        OnMiniGameSuccess -= HandleSuccess;
        OnMiniGameFail -= HandleFail;
    }

    public void StartGame(int index)
    {
        currentGameIndex = index;
        Debug.Log($"[MiniGameManager] 미니게임 {index} 시작");

        switch (index)
        {
            case 1: OnGame1Start?.Invoke(); break;
            case 2: OnGame2Start?.Invoke(); break;
            case 3: OnGame3Start?.Invoke(); break;
        }
    }

    // 외부에서 성공/실패 알릴 때 사용
    public static void NotifySuccess() => OnMiniGameSuccess?.Invoke();
    public static void NotifyFail()    => OnMiniGameFail?.Invoke();

    private void HandleSuccess()
    {
        Debug.Log($"[MiniGameManager] 게임 {currentGameIndex} 성공");

        if (currentGameIndex >= totalGames)
        {
            OnAllGamesCleared?.Invoke();
            Debug.Log("[MiniGameManager] 모든 미니게임 클리어!");
            return;
        }

        StartGame(currentGameIndex + 1);
    }

    private void HandleFail()
    {
        Debug.Log($"[MiniGameManager] 게임 {currentGameIndex} 실패 — 재시작");
        StartGame(currentGameIndex);
    }
}
