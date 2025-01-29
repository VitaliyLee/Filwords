using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> scoreText;

    [Space(10)]
    [SerializeField] private TextMeshProUGUI noobGuessedWords;
    [SerializeField] private TextMeshProUGUI normalGuessedWords;
    [SerializeField] private TextMeshProUGUI veteranGuessedWords;
    [SerializeField] private TextMeshProUGUI professionalGuessedWords;

    private int scoreRate = 2;//Множитель при отнятии очков за уровень

    private int score;

    public int Score { get => score; }

    public void AddScore(int Score)
    {
        score += Score;
        for (int i = 0; i < scoreText.Count; i++)
            scoreText[i].text = score.ToString();
    }

    public int AddScoreForAnswer(int LettersCount, int AnswerMinutes)
    {
        int scoreToLevel = 0;
        if(AnswerMinutes >= LettersCount)
            return scoreToLevel;

        scoreToLevel = LettersCount - AnswerMinutes * scoreRate;
        score += scoreToLevel;
        for (int i = 0; i < scoreText.Count; i++)
            scoreText[i].text = score.ToString();

        return scoreToLevel;
    }

    public void SetGuessedWords()
    {
        //3, 4, 5, 6 - соответственно обозначают количество слов на уровне. Их всегда будет столько, но архитектуру я все равно ГАВНО сделал (
        noobGuessedWords.text = $"{Saver.LevelNoob * 3} из 18";
        normalGuessedWords.text = $"{Saver.LevelNormal * 4} из 44";
        veteranGuessedWords.text = $"{Saver.LevelVeteran * 5} из 70";
        professionalGuessedWords.text = $"{Saver.LevelProfessionsl * 6} из 90";
    }
}
