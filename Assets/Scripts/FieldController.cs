using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class FieldController : MonoBehaviour
{
    [SerializeField] private string xmlFileName; // Имя файла без расширения
    [SerializeField] private int wordsCount;
    [SerializeField] private List<CardData> cardsList;

    private DictionaryController dictionaryController; //Класс управляющий словарями
    private Dictionary<string, string> dictionary; //Собственно сам словарь

    private List<string> selectedWordsList;//Список слов на уровне
    private CardData[,] cardsMatrix;//Нужен что бы из cardsList сделать матрицу для удобства работы

    public List<string> SelectedWordsList { get => selectedWordsList; }

    private void Start()
    {
        selectedWordsList = new List<string>();
        dictionaryController = new DictionaryController();
        dictionary = dictionaryController.GetDictionary(xmlFileName);

        InitializeCardsMatrix(wordsCount);
        SelectWords();
        FillPlayingField(wordsCount);
    }

    //Что бы удобнее было работать с карточками букв, запихиваем их в матрицу
    private void InitializeCardsMatrix(int fieldScale)
    {
        //индекс нужен потому, что проще сопоставить список с матрицей не смог 
        int listIndex = 0;

        cardsMatrix = new CardData[fieldScale, fieldScale];

        for (int i = 0; i < fieldScale; i++)
        {
            for (int j = 0; j < fieldScale; j++)
            {
                cardsMatrix[i, j] = cardsList[listIndex];
                cardsMatrix[i, j].cardIndex = new Vector2Int(i, j);

                listIndex++;
            }
        }
    }

    //Выбирает слова для уровня
    private void SelectWords()
    {
        int wordIndex = 0;
        string word = "";

        //Выбираем слова из списка так, что бы все 5 были уникальными
        while (selectedWordsList.Count < wordsCount)
        {
            wordIndex = Random.Range(0, dictionary.Count);
            word = dictionaryController.GetTextById(wordIndex.ToString());

            //Продолжаем цикл, если в списке уже есть такое слово
            if (!selectedWordsList.Contains(word))
                selectedWordsList.Add(word);
        }
    }

    //Пока просто перебиваем матрицу с буквами в карточки
    private void FillPlayingField(int fieldScale)
    {
        int x, y= 0;
        int patternIndex = 0;
        //Vector2Int[,] currentMatrix = Patterns.Rotate90(Patterns.PatternsList[patternIndex]);
        Vector2Int[,] currentMatrix = Patterns.PatternsList[patternIndex];


        for (int i = 0; i < fieldScale; i++)
        {
            for (int j = 0; j < fieldScale; j++)
            {
                x = currentMatrix[i, j].x;
                y = currentMatrix[i, j].y;

                cardsMatrix[y, fieldScale - x - 1].textLetter.text = selectedWordsList[i][fieldScale - j - 1].ToString();
            }
            Debug.Log(selectedWordsList[i]);
        }
    }

    //Делаю паттерн туда-сюда

    //Поворот паттернов

    //1. cardsMatrix[x, y].textLetter.text = selectedWordsList[i][j].ToString(); - обычное заполнение 

    //2. cardsMatrix[y, fieldScale - x - 1].textLetter.text = selectedWordsList[i][j].ToString(); - Обычный поворот на 90 градусов по часовой стрелке

    //3. cardsMatrix[fieldScale - y - 1, x].textLetter.text = selectedWordsList[i][j].ToString(); - Обычный поворот на 90 градусов против часовой стрелки

    //4. cardsMatrix[fieldScale - x - 1, fieldScale - y - 1].textLetter.text = selectedWordsList[i][j].ToString(); - Обычный поворот на 180 градусов

    //Отражение паттернов с поворотом

    //5. cardsMatrix[x, fieldScale - y - 1].textLetter.text = selectedWordsList[i][j].ToString(); - Отзеркаливает паттерн

    //6. cardsMatrix[fieldScale - y - 1, fieldScale - x - 1].textLetter.text = selectedWordsList[i][j].ToString(); - Отзеркаливает и поворачивает на 90 градусов по часовой стрелке

    //7. cardsMatrix[y, x].textLetter.text = selectedWordsList[i][j].ToString(); - Поворачиват на 90 градусов против часовой стрелки и отзеркаливает паттерн

    //8. cardsMatrix[fieldScale - x - 1, y].textLetter.text = selectedWordsList[i][j].ToString(); - Поворачивает на 180 градусов и отзеркаливает паттерн


    //9. cardsMatrix[x, y].textLetter.text = selectedWordsList[i][fieldScale - j - 1].ToString(); - Делает слова в задом наперед. Что бы комбинировать с другими модификациями нужно заменить на selectedWordsList[i][fieldScale - j - 1].ToString() выбранную букву
}
