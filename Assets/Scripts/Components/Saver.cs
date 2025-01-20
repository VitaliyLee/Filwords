using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Saver
{
    public static int LevelNoob { get; private set; }
    public static int LevelNormal { get; private set; }
    public static int LevelVeteran { get; private set; }
    public static int LevelProfessionsl { get; private set; }

    public static int Score { get; private set; }

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

    public static void Load()
    {
        LevelNoob = 0;
        LevelNormal = 1;
        LevelVeteran = 2;
        LevelProfessionsl = 3;

        Score = 100;
    }
}
