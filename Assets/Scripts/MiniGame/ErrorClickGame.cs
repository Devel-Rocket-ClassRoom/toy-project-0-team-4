using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorClickGame : MonoBehaviour
{
    [Header("에러 팝업 설정")]
    public GameObject errorPopupPrefab;
    public GameObject specialPopupPrefab;
    public Transform popupContainer;
    public float spawnInterval = 0f; // 0이면 한 프레임에 전부 생성

    [Header("배치 설정")]
    public int groupCount = 4;
    public int popupsPerGroup = 10;
    public Vector2 offsetStep = new Vector2(25f, -25f);
    public Vector2 groupOffset = new Vector2(300f, 0f);
    public Vector2 startPosition = new Vector2(0f, 300f);

    [Header("동의/거절 UI")]
    public GameObject agreeRejectPanel;
    public Button agreeButton;
    public Button rejectButton;

    private List<GameObject> spawnedPopups = new List<GameObject>();
    private bool isFinished;

    public void StartMiniGame()
    {
        isFinished = false;

        if (agreeRejectPanel != null)
            agreeRejectPanel.SetActive(true);
        if (agreeButton != null)
        {
            agreeButton.interactable = false;
            agreeButton.onClick.RemoveAllListeners();
            agreeButton.onClick.AddListener(OnClickAgree);
        }
        if (rejectButton != null)
        {
            rejectButton.onClick.RemoveAllListeners();
            rejectButton.onClick.AddListener(OnClickReject);
        }

        if (!gameObject.activeInHierarchy)
            gameObject.SetActive(true);

        StartCoroutine(SpawnPopupsSequence());
    }

    IEnumerator SpawnPopupsSequence()
    {
        int totalCount = groupCount * popupsPerGroup;
        // 앞 3개, 뒤 3개 제외하고 랜덤 위치에 특수 팝업
        int specialTotal = Random.Range(3, totalCount - 3);

        for (int g = 0; g < groupCount; g++)
        {
            Vector2 groupStart = startPosition + new Vector2(g * groupOffset.x, g * groupOffset.y);

            for (int i = 0; i < popupsPerGroup; i++)
            {
                int currentTotal = g * popupsPerGroup + i;
                Vector2 pos = groupStart + offsetStep * i;
                bool isSpecial = (currentTotal == specialTotal);

                GameObject prefab =
                    (isSpecial && specialPopupPrefab != null)
                        ? specialPopupPrefab
                        : errorPopupPrefab;
                GameObject popup = Instantiate(prefab, popupContainer);
                popup.GetComponent<RectTransform>().anchoredPosition = pos;

                spawnedPopups.Add(popup);

                Button[] btns = popup.GetComponentsInChildren<Button>(true);
                GameObject currentPopup = popup;

                if (isSpecial && btns.Length >= 2)
                {
                    btns[0].onClick.AddListener(() => OnClickPopup(currentPopup)); // 확인
                    btns[1].onClick.AddListener(OnClickReject); // 거절
                }
                else if (btns.Length >= 1)
                {
                    btns[0].onClick.AddListener(() => OnClickPopup(currentPopup));
                }

                if (spawnInterval > 0f)
                    yield return new WaitForSeconds(spawnInterval);
                else
                    yield return null; // 한 프레임씩만 대기
            }
        }
    }

    void OnClickPopup(GameObject popup)
    {
        if (isFinished)
            return;

        spawnedPopups.Remove(popup);
        Destroy(popup);

        if (spawnedPopups.Count == 0)
            if (agreeButton != null)
                agreeButton.interactable = true;
    }

    void OnClickAgree()
    {
        if (isFinished)
            return;
        isFinished = true;
        MiniGameManager.NotifySuccess();
    }

    void OnClickReject()
    {
        if (isFinished)
            return;
        isFinished = true;

        foreach (var p in spawnedPopups)
            if (p != null)
                Destroy(p);
        spawnedPopups.Clear();

        MiniGameManager.NotifyFail();
    }
}
