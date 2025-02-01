using GamePush;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static class Saver
{
    public delegate void AchieveSaveHendler(string AchieveKey);
    public static event AchieveSaveHendler AchieveSaveEvent;
    public delegate void LoadHendler();
    public static event LoadHendler LoadEvent;

    public static int LevelNoob { get; private set; }
    public static int LevelNormal { get; private set; }
    public static int LevelVeteran { get; private set; }
    public static int LevelProfessionsl { get; private set; }
    public static int Score { get; private set; }
    public static string AchieveKeysString { get; private set; }

    public static void ResettingLevelProgress()
    {
        LevelNoob = 0;
        LevelNormal = 0;
        LevelVeteran = 0;
        LevelProfessionsl = 0;

        GP_Player.Set("LevelNoob", 0);
        GP_Player.Set("LevelNormal", 0);
        GP_Player.Set("LevelVeteran", 0);
        GP_Player.Set("LevelProfessionsl", 0);

        GP_Player.Sync(storage: SyncStorageType.preffered);
    }

    public static void SaveValue(string Key, int Value)
    {
        switch (Key)
        {
            case "LevelNoob":
                LevelNoob = Value;
                break;
            case "LevelNormal":
                LevelNormal = Value;
                break;
            case "LevelVeteran":
                LevelVeteran = Value;
                break;
            case "LevelProfessionsl":
                LevelProfessionsl = Value;
                break;
        }

        GP_Player.Set(Key, Value);
        GP_Player.Sync(storage: SyncStorageType.preffered);
    }

    public static void SaveWithScore(string Key, int Value, int ScoreValue)
    {
        switch (Key)
        {
            case "LevelNoob":
                LevelNoob = Value;
                break;
            case "LevelNormal":
                LevelNormal = Value;
                break;
            case "LevelVeteran":
                LevelVeteran = Value;
                break;
            case "LevelProfessionsl":
                LevelProfessionsl = Value;
                break;
        }

        Score = ScoreValue;

        GP_Player.Set(Key, Value);
        GP_Player.Set("Score", ScoreValue);

        GP_Player.Sync(storage: SyncStorageType.preffered);
    }

    public static void SaveAchieve(string Key)
    {
        if (!AchieveKeysString.Contains(Key))
        {
            AchieveKeysString += $",{Key}";
            AchieveSaveEvent?.Invoke(Key);
            GP_Player.Set("AchieveKeysString", AchieveKeysString );
        }
    }

    public static void Load()
    {
        LevelNoob = GP_Player.GetInt("LevelNoob");
        LevelNormal = GP_Player.GetInt("LevelNormal");
        LevelVeteran = GP_Player.GetInt("LevelVeteran");
        LevelProfessionsl = GP_Player.GetInt("LevelProfessionsl");
        Score = GP_Player.GetInt("Score");

        LevelNoob = 7;
        LevelNormal = 9;
        LevelVeteran = 20;
        LevelProfessionsl = 5;
        Score = GP_Player.GetInt("Score");

        AchieveKeysString = GP_Player.GetString("AchieveKeysString") ?? "";
        LoadEvent?.Invoke();
    }
    //Все ключи соответствуют названиям полей этого класса
}
