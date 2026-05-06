using UnityEngine;
using TMPro;

public class BeadGameManager : MonoBehaviour
{
    public static BeadGameManager Current { get; private set; }

    [Header("UI 연결")]
    [SerializeField] private TextMeshProUGUI agreeButtonText;

    [Header("데이터")]
    [SerializeField] private string targetWord = "AGREE";

    private string currentWord = "";
    private string wrongWord = "";

    public string CurrentWord => currentWord;
    public string WrongWord => wrongWord;
    public string TargetWord => targetWord;

    [Header("스테이지 화면")]
    [SerializeField] private StageScreen stageScreen;

    private void Awake()
    {
        Current = this;

        if (stageScreen == null)
        {
            stageScreen = GetComponentInParent<StageScreen>(true);
        }
        ResetWord();
    }

    private void OnEnable()
    {
        Current = this;
    }

    private void OnDisable()
    {
        if (Current == this)
        {
            Current = null;
        }
    }

    public void AddLetter(char letter)
    {
        currentWord += letter;
        agreeButtonText.text = currentWord;
    }

    public void CheckResult()
    {
        Debug.Log($"CheckResult 실행 / 현재 단어: [{currentWord}] / 목표 단어: [{targetWord}]");

        if (currentWord == targetWord)
        {
            Debug.Log("성공: 이용약관에 동의했습니다.");

            if (stageScreen == null)
            {
                stageScreen = GetComponentInParent<StageScreen>(true);
            }

            if (stageScreen != null)
            {
                stageScreen.ClearStage();
            }
            return;
        }

        Debug.Log($"실패: {currentWord}는 {targetWord}와 다름");

        // ResetWord 전에 실패 단어 저장
        wrongWord = currentWord;

        ShowErrorPopup(wrongWord);

        ResetWord();
    }

    private void ShowErrorPopup(string word)
    {
        BeadErrorPopupUI.Instance.Show(word);
    }

    public void ResetWord()
    {
        currentWord = "";
        agreeButtonText.text = "";
    }
}