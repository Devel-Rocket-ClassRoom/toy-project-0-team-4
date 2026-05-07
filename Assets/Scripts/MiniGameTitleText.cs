using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MiniGameTitleText : MonoBehaviour
{
    private TMP_Text tmpText;
    private Text uiText;

    private void Awake()
    {
        tmpText = GetComponent<TMP_Text>();
        uiText = GetComponent<Text>();
    }

    public void SetTitle(string title)
    {
        if (tmpText != null)
        {
            tmpText.text = title;
            return;
        }

        if (uiText != null)
        {
            uiText.text = title;
            return;
        }

        Debug.LogWarning($"{gameObject.name}에 TMP_Text 또는 Text 컴포넌트가 없습니다.");
    }
}