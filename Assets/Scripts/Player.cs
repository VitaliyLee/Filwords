using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    [Space(10)]
    [SerializeField] private TextMeshProUGUI noobGuessedWords;
    [SerializeField] private TextMeshProUGUI normalGuessedWords;
    [SerializeField] private TextMeshProUGUI veteranGuessedWords;
    [SerializeField] private TextMeshProUGUI professionalGuessedWords;

    private int score;

    public int Score { get => score; }

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

    public void SetGuessedWords()
    {
        //3, 4, 5, 6 - соответственно обозначают количество слов на уровне. Их всегда будет столько, но архитектуру я все равно ГАВНО сделал (
        noobGuessedWords.text = $"{Saver.LevelNoob * 3} из 20";
        normalGuessedWords.text = $"{Saver.LevelNormal * 4} из 20";
        veteranGuessedWords.text = $"{Saver.LevelVeteran * 5} из 20";
        professionalGuessedWords.text = $"{Saver.LevelProfessionsl * 6} из 20";
    }
}
