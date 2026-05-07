using UnityEngine;
using TMPro;

public class BeadErrorPopupUI : MonoBehaviour
{
    public static BeadErrorPopupUI Instance { get; private set; }

    [Header("팝업 루트")]
    [SerializeField] private GameObject popupRoot;

    [Header("의미불명 팝업")]
    [SerializeField] private GameObject errorPanel;

    [Header("팝업 문구 텍스트")]
    [SerializeField] private TMP_Text errorContentText;

    private void Awake()
    {
        Instance = this;
        errorPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void Show(string wrongWord)
    {
        popupRoot.SetActive(true);
        errorContentText.text = $"[{wrongWord}]";
        errorPanel.SetActive(true);
    }

    public void Hide()
    {
        errorPanel.SetActive(false);
        popupRoot.SetActive(false);
    }
}