using GamePush;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Ad
{
    private static Action actionAfterRewordedAd;
    private static int timeAfterLastFullscreenCalling;
    private static int timeFullscreenRate = 120;

    #region Reworded
    public static void ShowRewardedAd(Action ActionAfterAd)
    {
        actionAfterRewordedAd = ActionAfterAd;
        GP_Ads.ShowRewarded("COINS", OnRewardedReward, OnRewardedStart, OnRewardedClose);
    }

    // Начался показ
    private static void OnRewardedStart() => Debug.Log("ON REWARDED: START");

    // Получена награда
    private static void OnRewardedReward(string value)
    {
        if (value == "COINS")
            actionAfterRewordedAd?.Invoke();

        if (value == "GEMS")
            Debug.Log("ON REWARDED: +5 GEMS");
    }

    // Закончился показ
    private static void OnRewardedClose(bool success) => Debug.Log("ON REWARDED: CLOSE");
    #endregion

    #region Fullscreen
    public static void ShowFullscreenAd(int gameTime)
    {
        timeAfterLastFullscreenCalling += gameTime;
        if (timeAfterLastFullscreenCalling >= timeFullscreenRate)
        {
            Debug.Log("Showing fullscreen!");
            GP_Ads.ShowFullscreen(OnFullscreenStart, OnFullscreenClose);
            timeAfterLastFullscreenCalling = 0;
        }
        else
            Debug.Log("Do not showing fullscreen!");
    }

    // Начался показ
    private static void OnFullscreenStart() => Debug.Log("ON FULLSCREEN START");
    // Закончился показ
    private static void OnFullscreenClose(bool success) => Debug.Log("ON FULLSCREEN CLOSE");
    #endregion
}
