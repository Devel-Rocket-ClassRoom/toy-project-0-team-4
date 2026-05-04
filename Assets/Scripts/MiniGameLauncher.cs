using UnityEngine;
using UnityEngine.UI;

// 미니게임 프리팹을 랜덤으로 생성하고, 성공/실패 이벤트를 받아 정리하는 컴포넌트
public class MiniGameLauncher : MonoBehaviour
{
    [SerializeField] private GameObject[] miniGamePrefabs; // Inspector에 MiniGame1, 2, 3 연결

    private GameObject currentGame; // 현재 실행 중인 미니게임 인스턴스

    void OnEnable()
    {
        // 미니게임 결과 이벤트 구독
        MiniGameManager.OnMiniGameSuccess += OnGameEnd;
        MiniGameManager.OnMiniGameFail    += OnGameEnd;
    }

    void OnDisable()
    {
        // 이벤트 구독 해제 (메모리 누수 방지)
        MiniGameManager.OnMiniGameSuccess -= OnGameEnd;
        MiniGameManager.OnMiniGameFail    -= OnGameEnd;
    }

    // 외부에서 호출: 랜덤 미니게임을 Canvas 하위에 생성하고 시작
    public void LaunchRandom()
    {
        if (miniGamePrefabs == null || miniGamePrefabs.Length == 0)
        {
            Debug.LogError("[MiniGameLauncher] miniGamePrefabs가 비어있음");
            return;
        }

        // 이미 실행 중인 게임이 있으면 제거
        if (currentGame != null)
            Destroy(currentGame);

        int index = Random.Range(0, miniGamePrefabs.Length);
        Canvas canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null) { Debug.LogError("[MiniGameLauncher] Canvas를 찾을 수 없음"); return; }

        currentGame = Instantiate(miniGamePrefabs[index], canvas.transform);

        MiniGameManager.Instance.StartGame(index + 1); // 프리팹 배열 0-based → gameIndex 1-based
    }

    // 미니게임 성공 또는 실패 시 호출: 현재 게임 오브젝트 파괴
    void OnGameEnd()
    {
        if (currentGame != null)
        {
            Destroy(currentGame);
            currentGame = null;
        }
    }
}
