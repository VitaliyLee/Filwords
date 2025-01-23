using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static class Saver
{
    public delegate void AchieveSaveHendler(string AchieveKey);
    public static event AchieveSaveHendler AchieveSaveEvent;

    public static int LevelNoob { get; private set; }
    public static int LevelNormal { get; private set; }
    public static int LevelVeteran { get; private set; }
    public static int LevelProfessionsl { get; private set; }
    public static int Score { get; private set; }
    public static string AchieveKeysString { get; private set; }

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
    }

    public static void SaveWithScore(string Key, int Value, int ScoreValue)
    {
        switch(Key)
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
    }

    public static void SaveAchieve(string Key)
    {
        if(!AchieveKeysString.Contains(Key))
        {
            AchieveKeysString += $",{Key}";
            AchieveSaveEvent?.Invoke(Key);
        }
    }

    public static void Load()
    {
        LevelNoob = 1;
        LevelNormal = 1;
        LevelVeteran = 1;
        LevelProfessionsl = 1;
        Score = 1001;

        AchieveKeysString = "1";
    }
}
