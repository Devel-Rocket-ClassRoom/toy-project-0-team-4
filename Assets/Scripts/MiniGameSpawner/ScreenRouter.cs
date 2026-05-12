using UnityEngine;

public class ScreenRouter
{
    private readonly GameObject titleScreen;
    private readonly GameObject mainScreen;

    public ScreenRouter(GameObject titleScreen, GameObject mainScreen)
    {
        this.titleScreen = titleScreen;
        this.mainScreen = mainScreen;
    }

    /// <summary>
    /// 타이틀 화면만 표시 (메인 화면은 숨김)
    /// </summary>
    public void ShowTitleOnly() => Apply(titleActive: true, mainActive: false);

    /// <summary>
    /// 타이틀과 메인 화면을 동시에 표시 (성공 후 메인으로 이동)
    /// </summary>
    public void ShowTitleWithMain() => Apply(titleActive: true, mainActive: true);

    private void Apply(bool titleActive, bool mainActive)
    {
        if (titleScreen != null)
        {
            titleScreen.SetActive(titleActive);
        }

        if (mainScreen != null)
        {
            mainScreen.SetActive(mainActive);
        }
    }
}
