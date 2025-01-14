using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using Random = UnityEngine.Random;

public class FieldController : MonoBehaviour
{
    [SerializeField] private int level;//На релизе поменять на закрытую переменую. Она будет СОХРАНЯТЬСЯ

    private int fieldSize;

    private List<CardData> cardsList;
    private Dictionary<string, string> dictionary; //Собственно сам словарь

    private List<string> selectedWordsList;//Список слов на уровне
    private CardData[,] cardsMatrix;//Нужен что бы из cardsList сделать матрицу для удобства работы

    Vector2Int[,] currentMatrix;

    private int patternModsCount = 8; //Колличество модификаторов паттерна

    public int Level { get => level; }
    public List<string> SelectedWordsList { get => selectedWordsList; }
    public int FieldSize { get => fieldSize; }
    public int PatternModsCount { get => patternModsCount; }
    public Vector2Int[,] CurrentMatrix { get => currentMatrix; }
    public List<CardData> CardsList { set => cardsList = value; }

    public void Init(string xmlDictionaryName)
    {
        fieldSize = (int)MathF.Sqrt(cardsList.Count);
        selectedWordsList = new List<string>();
        dictionary = new DictionaryController().GetDictionary(xmlDictionaryName);

        InitializeCardsMatrix(fieldSize);
        SelectWords();
        FillPlayingField(fieldSize);
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
        if (level > dictionary.Count - 1) level = 0;

        int wordIndex = level * fieldSize;
        string word = "";

        //Выбираем слова из списка так, что бы все 5 были уникальными
        while (selectedWordsList.Count < fieldSize)
        {
            dictionary.TryGetValue(wordIndex.ToString(), out word);

            //Продолжаем цикл, если в списке уже есть такое слово
            if (!selectedWordsList.Contains(word))
                selectedWordsList.Add(word);

            wordIndex++;
        }
    }

    //Пока просто перебиваем матрицу с буквами в карточки
    private void FillPlayingField(int fieldScale)
    {
        currentMatrix = GetFieldMatrix();
        int patternModIndex = level % patternModsCount;
        int x, y = 0;

        for (int i = 0; i < fieldScale; i++)
        {
            for (int j = 0; j < fieldScale; j++)
            {
                x = currentMatrix[i, j].x;
                y = currentMatrix[i, j].y;

                switch (patternModIndex)
                {
                    case 0:
                        cardsMatrix[x, y].textLetter.text = selectedWordsList[i][j].ToString();
                        break;
                    case 1:
                        cardsMatrix[y, fieldScale - x - 1].textLetter.text = selectedWordsList[i][j].ToString();
                        break;
                    case 2:
                        cardsMatrix[fieldScale - y - 1, x].textLetter.text = selectedWordsList[i][j].ToString();
                        break;
                    case 3:
                        cardsMatrix[fieldScale - x - 1, fieldScale - y - 1].textLetter.text = selectedWordsList[i][j].ToString();
                        break;
                    case 4:
                        cardsMatrix[x, fieldScale - y - 1].textLetter.text = selectedWordsList[i][j].ToString();
                        break;
                    case 5:
                        cardsMatrix[fieldScale - y - 1, fieldScale - x - 1].textLetter.text = selectedWordsList[i][j].ToString();
                        break;
                    case 6:
                        cardsMatrix[y, x].textLetter.text = selectedWordsList[i][j].ToString();
                        break;
                    case 7:
                        cardsMatrix[fieldScale - x - 1, y].textLetter.text = selectedWordsList[i][j].ToString();
                        break;
                }
            }
            Debug.Log(selectedWordsList[i]);
        }
    }

    private Vector2Int[,] GetFieldMatrix()
    {
        int patternIndex = 0;

        if (fieldSize == 4)
        {
            patternIndex = (level / patternModsCount) % Patterns.PatternsFourLettersList.Count;
            return Patterns.PatternsFourLettersList[patternIndex];
        }

        else
        {
            patternIndex = (level / patternModsCount) % Patterns.PatternsList.Count;
            return Patterns.PatternsList[patternIndex];
        }
    }

    public void NewLevel()
    {
        level++;
        selectedWordsList.Clear();

        SelectWords();
        FillPlayingField(fieldSize);
    }

    //Делаю паттерн туда-сюда 72

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
