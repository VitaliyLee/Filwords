using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] int score;
    [SerializeField] TextMeshProUGUI scoreText;

    public void AddScore(int Score)
    {
        score += Score;
        scoreText.text = score.ToString();
    }

    public void AddScoreForAnswer(int LettersCount, int answerMinutes)
    {
        if(answerMinutes >= LettersCount)
            return;

        score += LettersCount - answerMinutes;
        scoreText.text = score.ToString();
    }

    public void SubtractScore(int value)
    {
        if(score < value)
        {
            Debug.Log("Show Reworded");
        }

        else
        {
            score -= value;
        }

        scoreText.text = score.ToString();
    }
}
