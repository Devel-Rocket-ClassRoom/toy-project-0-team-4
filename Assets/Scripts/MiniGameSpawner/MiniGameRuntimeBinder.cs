using System.Reflection;
using UnityEngine;

/// <summary>
/// buttonHandler 자동 연결 + StartMiniGame 자동 실행
/// </summary>

public static class MiniGameRuntimeBinder
{
    public static void BindButtonHandler(StageScreen currentMiniGame, OnClickButton onClickButton)
    {
        if (currentMiniGame == null)
        {
            return;
        }

        if (onClickButton == null)
        {
            Debug.LogWarning("OnClickButton이 연결되지 않았습니다.");
            return;
        }

        // 생성된 미니게임 안의 모든 MonoBehaviour를 검사
        MonoBehaviour[] behaviours = currentMiniGame.GetComponentsInChildren<MonoBehaviour>(true);

        int bindCount = 0;

        foreach (MonoBehaviour behaviour in behaviours)
        {
            if (behaviour == null)
            {
                continue;
            }

            // buttonHandler라는 필드를 찾음
            FieldInfo field = behaviour.GetType().GetField(
                "buttonHandler",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            );

            if (field == null)
            {
                continue;
            }

            // buttonHandler 타입이 OnClickButton이면 자동 연결
            if (field.FieldType == typeof(OnClickButton))
            {
                field.SetValue(behaviour, onClickButton);
                bindCount++;
            }
        }
    }

    public static void TryStartMiniGame(StageScreen currentMiniGame)
    {
        if (currentMiniGame == null)
        {
            return;
        }

        // 생성된 미니게임 안에 StartMiniGame() 메서드가 있으면 실행
        MonoBehaviour[] behaviours = currentMiniGame.GetComponentsInChildren<MonoBehaviour>(true);

        foreach (MonoBehaviour behaviour in behaviours)
        {
            if (behaviour == null)
            {
                continue;
            }

            MethodInfo method = behaviour.GetType().GetMethod(
                "StartMiniGame",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            );

            if (method == null)
            {
                continue;
            }

            // 매개변수가 없는 StartMiniGame만 실행
            if (method.GetParameters().Length > 0)
            {
                continue;
            }

            method.Invoke(behaviour, null);

            return;
        }
    }
}