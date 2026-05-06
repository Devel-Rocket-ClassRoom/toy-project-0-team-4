using UnityEngine;
using TMPro;

public class BeadGameManager : MonoBehaviour
{
    [Header("UI 연결")]
    public TextMeshProUGUI agreeButtonText; // 하단 초록 버튼 텍스트
    //public GameObject errorPanel;           // 오답 팝업
    //public TextMeshProUGUI errorContentText; // 팝업 내 문자열 표시

    [Header("데이터")]
    public string targetWord = "AGREE";
    private string currentWord = "";

    public void AddLetter(char letter)
    {
        currentWord += letter;
        agreeButtonText.text = currentWord;
    }

    public void CheckResult()
    {
        if (currentWord == targetWord)
        {
            Debug.Log("성공: 이용약관에 동의했습니다.");
            // 성공 시 로직 (예: 미니게임 파괴 및 다음 단계)
        }
        else
        {
            Debug.Log($"실패: {currentWord}는 {targetWord}와 다름");
            ShowError();
        }
    }

    void ShowError()
    {
        //errorContentText.text = $"[{currentWord}]\n버튼의 내용이 의미불명입니다.";
        //errorPanel.SetActive(true);
        ResetWord();
    }

    public void ResetWord()
    {
        currentWord = "";
        agreeButtonText.text = "";
    }
}