using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

[Serializable]
public class TermItem
{
    public int id;
    public string article;
    public string title;
    public string content;
}

[Serializable]
public class TermsData
{
    public string documentTitle;
    public string effectiveDate;
    public List<TermItem> termsList;
}

public class JsonLoader : MonoBehaviour
{
    [Header("UI References (UGUI)")]
    // Canvas UI 시스템에서는 TextMeshProUGUI를 사용합니다.
    [SerializeField]
    private TextMeshProUGUI titleText;

    [SerializeField]
    private TextMeshProUGUI effectiveDateText;

    [SerializeField]
    private TextMeshProUGUI contentText;

    [Header("Data File")]
    [SerializeField]
    private string fileName = "GameTexts";

    void Start()
    {
        LoadAndDisplayTerms();
    }

    private void LoadAndDisplayTerms()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(fileName);

        if (jsonFile == null)
        {
            Debug.LogError($"'{fileName}' 파일을 찾을 수 없습니다.");
            return;
        }

        // JSON 파싱
        TermsData data = JsonUtility.FromJson<TermsData>(jsonFile.text);

        // UI 텍스트 업데이트
        if (titleText != null)
            titleText.text = data.documentTitle;
        if (effectiveDateText != null)
            effectiveDateText.text = $"시행일: {data.effectiveDate}";

        // 본문 내용 최적화 합치기
        StringBuilder sb = new StringBuilder();
        foreach (var item in data.termsList)
        {
            sb.AppendLine($"<b>{item.article} ({item.title})</b>");
            sb.AppendLine(item.content);
            sb.AppendLine();
        }

        if (contentText != null)
        {
            contentText.text = sb.ToString();
        }
    }
}
