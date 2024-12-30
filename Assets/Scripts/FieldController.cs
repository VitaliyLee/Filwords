using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class FieldController : MonoBehaviour
{
    [SerializeField] private string xmlFileName; // »м€ файла без расширени€
    [SerializeField] private int wordsCount;
    [SerializeField] private List<CardController> cardsList;

    private DictionaryController dictionaryController; // ласс управл€ющий словар€ми
    private Dictionary<string, string> dictionary; //—обственно сам словарь

    private List<string> selectedWordsList;//—писок слов на уровне
    private CardController[,] cardsMatrix;//Ќужен что бы из cardsList сделать матрицу дл€ удобства работы

    private void Start()
    {
        selectedWordsList = new List<string>();
        dictionaryController = new DictionaryController();
        dictionary = dictionaryController.GetDictionary(xmlFileName);

        InitializeCardsMatrix(wordsCount);
        SelectWords();
        FillPlayingField(wordsCount);
    }

    //„то бы удобнее было работать с карточками букв, запихиваем их в матрицу
    private void InitializeCardsMatrix(int fieldScale)
    {
        //индекс нужен потому, что проще сопоставить список с матрицей не смог 
        int listIndex = 0;

        cardsMatrix = new CardController[fieldScale, fieldScale];

        for (int i = 0; i < fieldScale; i++)
        {
            for (int j = 0; j < fieldScale; j++)
            {
                cardsMatrix[i, j] = cardsList[listIndex];
                listIndex++;
            }
        }
    }

    private void SelectWords()
    {
        int wordIndex = 0;
        string word = "";

        //¬ыбираем слова из списка так, что бы все 5 были уникальными
        while (selectedWordsList.Count < wordsCount)
        {
            wordIndex = Random.Range(0, dictionary.Count);
            word = dictionaryController.GetTextById(wordIndex.ToString());

            //ѕродолжаем цикл, если в списке уже есть такое слово
            if (!selectedWordsList.Contains(word))
                selectedWordsList.Add(word);
        }
    }

    //ѕока просто перебиваем матрицу с буквами в карточки
    private void FillPlayingField(int fieldScale)
    {
        int x= 0;
        int y= 0;

        for (int i = 0; i < fieldScale; i++)
        {
            for (int j = 0; j < fieldScale; j++)
            {
                x = Patterns.PatternsList[0][i, j].x;
                y = Patterns.PatternsList[0][i, j].y;

                cardsMatrix[x, y].textLetter.text = selectedWordsList[i][j].ToString();
            }
        }
    }
}
