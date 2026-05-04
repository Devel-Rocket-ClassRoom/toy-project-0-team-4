using UnityEngine;
using System;

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager Instance { get; private set; }

    public static event Action OnGame1Start;
    public static event Action OnGame2Start;
    public static event Action OnGame3Start;
    public static event Action OnGame4Start;
    public static event Action OnGame5Start;
    public static event Action OnMiniGameSuccess;
    public static event Action OnMiniGameFail;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void ResetStaticState()
    {
        Instance        = null;
        OnGame1Start    = null;
        OnGame2Start    = null;
        OnGame3Start    = null;
        OnGame4Start    = null;
        OnGame5Start    = null;
        OnMiniGameSuccess = null;
        OnMiniGameFail    = null;
    }

    void Awake()
    {
        if (Instance != null) { Destroy(this); return; }
        Instance = this;
    }

    public void StartGame(int index)
    {
        switch (index)
        {
            case 1: OnGame1Start?.Invoke(); break;
            case 2: OnGame2Start?.Invoke(); break;
            case 3: OnGame3Start?.Invoke(); break;
            case 4: OnGame4Start?.Invoke(); break;
            case 5: OnGame5Start?.Invoke(); break;
            default: Debug.LogWarning($"[MiniGameManager] 처리되지 않은 gameIndex: {index}"); break;
        }
    }

    public static void NotifySuccess() => OnMiniGameSuccess?.Invoke();
    public static void NotifyFail()    => OnMiniGameFail?.Invoke();
}
