using UnityEngine;
using System;

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager Instance { get; private set; }

    public static event Action OnGame1Start;
    public static event Action OnGame2Start;
    public static event Action OnGame3Start;
    public static event Action OnMiniGameSuccess;
    public static event Action OnMiniGameFail;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void StartGame(int index)
    {
        switch (index)
        {
            case 1: OnGame1Start?.Invoke(); break;
            case 2: OnGame2Start?.Invoke(); break;
            case 3: OnGame3Start?.Invoke(); break;
        }
    }

    public static void NotifySuccess() => OnMiniGameSuccess?.Invoke();
    public static void NotifyFail()    => OnMiniGameFail?.Invoke();
}
