using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollAgreeMiniGame : MonoBehaviour
{
    [Header("UI 참조 (Inspector에서 연결)")]
    [SerializeField] private Button agreeButton;
    [SerializeField] private Button disagreeButton;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private TextMeshProUGUI contentTMP;

    [Header("JSON 설정")]
    [SerializeField] private string jsonFileName = "GameTexts"; // Resources 폴더 내 파일명

    [Header("비활성 상태 투명도 (0=완전투명, 1=불투명)")]
    [SerializeField] [Range(0f, 1f)] private float disabledAlpha = 0.3f;

    [Header("스크롤 감도")]
    [SerializeField] private float scrollSensitivity = 3f;

    [Header("활성화 오프셋 (양수=더 내려야 활성화)")]
    [SerializeField] private float activateOffset = 30f;

    private float textHeight;

    public void StartMiniGame()
    {
        if (scrollRect == null)
            scrollRect = GetComponentInChildren<ScrollRect>(true);

        scrollRect.scrollSensitivity = scrollSensitivity;

        SetAgreeButtonState(false);

        disagreeButton.onClick.AddListener(() => MiniGameManager.NotifyFail());
        agreeButton.onClick.AddListener(() => MiniGameManager.NotifySuccess());

        DisableDrag();

        LoadAndCombineText();

        StartCoroutine(FitContent());
    }

    private void LoadAndCombineText()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonFileName);
        if (jsonFile == null) return;

        TermsData data = JsonUtility.FromJson<TermsData>(jsonFile.text);
        StringBuilder sb = new StringBuilder();

        // 1. 큰 제목
        sb.AppendLine($"<size=140%><b>{data.documentTitle}</b></size>");
        // 2. 시행일
        sb.AppendLine($"<size=90%><color=#aaaaaa>최종 수정일: {data.effectiveDate}</color></size>");
        sb.AppendLine();
        sb.AppendLine();

        // 3. 모든 조항 순회하며 합치기
        foreach (var item in data.termsList)
        {
            sb.AppendLine($"<b>{item.article} ({item.title})</b>");
            sb.AppendLine(item.content);
            sb.AppendLine(); // 조항 간 간격
        }

        if (contentTMP != null)
            contentTMP.text = sb.ToString();
    }

    private void SetAgreeButtonState(bool active)
    {
        agreeButton.interactable = active;

        if (!agreeButton.TryGetComponent<CanvasGroup>(out var canvasGroup))
            canvasGroup = agreeButton.gameObject.AddComponent<CanvasGroup>();

        canvasGroup.alpha = active ? 1f : disabledAlpha;
    }

    private IEnumerator FitContent()
    {
        yield return new WaitForEndOfFrame();
        if (contentTMP == null) yield break;

        contentTMP.ForceMeshUpdate();

        var info = contentTMP.textInfo;
        if (info != null && info.lineCount > 0)
        {
            // 마지막 줄 하단 위치 계산
            var lastLine = info.lineInfo[info.lineCount - 1];
            textHeight = Mathf.Abs(lastLine.descender);
        }
        else
        {
            textHeight = contentTMP.preferredHeight;
        }

        // 텍스트와 컨텐츠 영역 크기 동기화
        contentTMP.rectTransform.sizeDelta = new Vector2(contentTMP.rectTransform.sizeDelta.x, textHeight);
        scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x, textHeight);

        yield return null;
        scrollRect.verticalNormalizedPosition = 1f; // 스크롤 맨 위로 초기화

        //if (scrollRect == null || scrollRect.content == null) yield break;

        //TextMeshProUGUI tmp = scrollRect.content.GetComponentInChildren<TextMeshProUGUI>();
        //if (tmp != null)
        //{
        //    tmp.ForceMeshUpdate();

        //    // 마지막 줄 하단 Y 위치로 실제 텍스트 높이 계산
        //    var info = tmp.textInfo;
        //    if (info != null && info.lineCount > 0)
        //    {
        //        var lastLine = info.lineInfo[info.lineCount - 1];
        //        // descender는 음수(위에서 아래 방향), 절댓값이 실제 높이
        //        textHeight = Mathf.Abs(lastLine.descender);
        //    }
        //    else
        //    {
        //        textHeight = tmp.preferredHeight;
        //    }

        //    tmp.rectTransform.sizeDelta = new Vector2(tmp.rectTransform.sizeDelta.x, textHeight);
        //    scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x, textHeight);
        //}

        //yield return null;
        //scrollRect.verticalNormalizedPosition = 1f;
    }

    private void DisableDrag()
    {
        if (scrollRect == null) return;

        if (scrollRect.verticalScrollbar != null)
            scrollRect.verticalScrollbar.interactable = false;

        scrollRect.horizontal = false;

        RectTransform viewport = scrollRect.viewport != null
            ? scrollRect.viewport
            : scrollRect.GetComponent<RectTransform>();

        GameObject blocker = new("DragBlocker", typeof(RectTransform));
        blocker.transform.SetParent(viewport, false);

        RectTransform rt = blocker.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        var img = blocker.AddComponent<Image>();
        img.color = Color.clear;
        img.raycastTarget = true;

        blocker.AddComponent<ScrollDragBlocker>();
    }

    private void Update()
    {
        if (scrollRect == null || agreeButton.interactable || textHeight <= 0f) return;

        float viewportH = scrollRect.viewport.rect.height;
        float scrolled = scrollRect.content.anchoredPosition.y;

        // 텍스트 끝이 뷰포트 하단에 완전히 들어오면 활성화
        if (scrolled >= textHeight - viewportH + activateOffset)
            SetAgreeButtonState(true);
    }
}
