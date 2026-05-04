using UnityEngine;
using UnityEngine.UI;

public class MiniGameLauncher : MonoBehaviour
{
    [SerializeField] private GameObject[] miniGamePrefabs; // Inspector에 MiniGame1, 2, 3 연결

    private GameObject currentGame;

    void OnEnable()
    {
        MiniGameManager.OnMiniGameSuccess += OnGameEnd;
        MiniGameManager.OnMiniGameFail    += OnGameEnd;
    }

    void OnDisable()
    {
        MiniGameManager.OnMiniGameSuccess -= OnGameEnd;
        MiniGameManager.OnMiniGameFail    -= OnGameEnd;
    }

    public void LaunchRandom()
    {
        if (miniGamePrefabs == null || miniGamePrefabs.Length == 0)
        {
            Debug.LogError("[MiniGameLauncher] miniGamePrefabs가 비어있음");
            return;
        }

        if (currentGame != null)
            Destroy(currentGame);

        int index = Random.Range(0, miniGamePrefabs.Length);
        Canvas canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null) { Debug.LogError("[MiniGameLauncher] Canvas를 찾을 수 없음"); return; }

        currentGame = Instantiate(miniGamePrefabs[index], canvas.transform);

        MiniGameManager.Instance.StartGame(index + 1); // 프리팹 배열 0-based → gameIndex 1-based
    }

    void OnGameEnd()
    {
        if (currentGame != null)
        {
            Destroy(currentGame);
            currentGame = null;
        }
    }
}
