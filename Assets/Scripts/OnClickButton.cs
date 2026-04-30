using UnityEngine;

public class OnClickButton : MonoBehaviour
{

    [SerializeField] private GameObject MainScreen;
    [SerializeField] private GameObject Stage1;
    [SerializeField] private GameObject Stage2;
    // [SerializeField] private GameObject Stage3;
    // [SerializeField] private GameObject Stage4;
    public void OnClickToMain()
    {
        // Main 씬으로 이동하는 코드 작성
        MainScreen.SetActive(true);
        Stage1.SetActive(false);
        Stage2.SetActive(false);
        // Stage3.SetActive(false);
        // Stage4.SetActive(false);
    }
    public void OnClickCancle()
    {
        // Title 씬으로 이동하는 코드 작성
        MainScreen.SetActive(false);
        Stage1.SetActive(false);
        Stage2.SetActive(false);
        // Stage3.SetActive(false);
        // Stage4.SetActive(false);
    }

    public void OnClickToStage1()
    {
        // Stage1 씬으로 이동하는 코드 작성
        MainScreen.SetActive(false);
        Stage1.SetActive(true);
        Stage2.SetActive(false);
    }
    public void OnClickToStage2()
    {
        // Stage2 씬으로 이동하는 코드 작성
        MainScreen.SetActive(false);
        Stage1.SetActive(false);
        Stage2.SetActive(true);
    }
    // public void OnClickToStage3()
    // {
    //     // Stage3 씬으로 이동하는 코드 작성
    //     MainScreen.SetActive(false);
    //     Stage1.SetActive(false);
    //     Stage2.SetActive(false);
    //     Stage3.SetActive(true);
    // }
    // public void OnClickToStage4()
    // {
    //     // Stage4 씬으로 이동하는 코드 작성
    //     MainScreen.SetActive(false);
    //     Stage1.SetActive(false);
    //     Stage2.SetActive(false);
    //     Stage3.SetActive(false);
    //     Stage4.SetActive(true);
    // }


}
