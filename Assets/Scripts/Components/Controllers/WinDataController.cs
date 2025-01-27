using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinDataController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI evaluationsText;
    [SerializeField] private TextMeshProUGUI playTimeText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image avatarImage;

    [Space(10)]
    [SerializeField] private List<EvaluationsDataSet> evaluationsDataList;

    private string TimeConverterToString(int PlayTime)
    {
        string timeString = "";
        int min = (int)(PlayTime / 60);
        int sec = (int)(PlayTime % 60);

        if (min < 10 && sec < 10)
            timeString = $"0{min}:0{sec}";
        else if (sec < 10)
            timeString = $"{min}:0{sec}";
        else if (min < 10)
            timeString = $"0{min}:{sec}";
        else
            timeString = $"{min}:{sec}";

        return timeString;
    }

    private EvaluationsDataSet SelectDataSet(float ScoreToLevel, float LevelLength)
    {
        float percentToMaxScore = (ScoreToLevel / LevelLength) * 100;

        if(percentToMaxScore > 75)
            return evaluationsDataList[0];
        if (percentToMaxScore < 75 && percentToMaxScore > 50)
            return evaluationsDataList[1];
        else
            return evaluationsDataList[2];
    }

    public void SetEvaluation(int PlayTime, int ScoreToLevel, int LevelLength)
    {
        EvaluationsDataSet selectedData = SelectDataSet(ScoreToLevel, LevelLength);

        evaluationsText.text = selectedData.EvaluationsString;
        playTimeText.text = TimeConverterToString(PlayTime);
        scoreText.text = ScoreToLevel.ToString();
        avatarImage.sprite = selectedData.EvaluationsSprite;
    }
}
