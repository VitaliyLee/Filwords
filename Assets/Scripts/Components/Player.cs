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

    [Space(10)]
    [SerializeField] private GameObject resetingLevelButton;
    [SerializeField] private List<LevelData> levelsData;

    private int scoreRate = 2;//��������� ��� ������� ����� �� �������

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
        //3, 4, 5, 6 - �������������� ���������� ���������� ���� �� ������. �� ������ ����� �������, �� ����������� � ��� ����� ����� ������ (
        noobGuessedWords.text = $"{Saver.LevelNoob * 3} �� 18";
        normalGuessedWords.text = $"{Saver.LevelNormal * 4} �� 44";
        veteranGuessedWords.text = $"{Saver.LevelVeteran * 5} �� 70";
        professionalGuessedWords.text = $"{Saver.LevelProfessionsl * 6} �� 90";
    }

    public void ResettingProgress()
    {
        Saver.ResettingLevelProgress();
        SetGuessedWords();
    }

    public void ViewResettingButton()
    {
        var dict = new DictionaryController();
        int index = 0;

        foreach(LevelData item in levelsData)
        {
            if (dict.GetDictionary(item.DictionaryName).Count == Saver.LevelNoob * 3)
            {
                index++;
            }
            if (dict.GetDictionary(item.DictionaryName).Count == Saver.LevelNormal * 4)
            {
                index++;
            }
            if (dict.GetDictionary(item.DictionaryName).Count == Saver.LevelVeteran * 5)
            {
                index++;
            }
            if (dict.GetDictionary(item.DictionaryName).Count == Saver.LevelProfessionsl * 6)
            {
                index++;
            }
        }

        if (index >= 4)
        {
            resetingLevelButton.SetActive(true);
        }
    }
}
