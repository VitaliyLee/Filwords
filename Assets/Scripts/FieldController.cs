using System;
using System.Collections.Generic;
using UnityEngine;

public class FieldController
{
    private int level;//На релизе поменять на закрытую переменую. Она будет СОХРАНЯТЬСЯ
    private int fieldSize;

    private List<CardData> cardsList;
    private Dictionary<string, string> dictionary; //Собственно сам словарь

    private List<string> selectedWordsList;//Список слов на уровне
    private CardData[,] cardsMatrix;//Нужен что бы из cardsList сделать матрицу для удобства работы

    Vector2Int[,] currentMatrix;
    private int patternModsCount = 8; //Колличество модификаторов паттерна

    public int Level { get => level; }
    public List<string> SelectedWordsList { get => selectedWordsList; }
    public List<CardData> CardsList { get => cardsList; }

    public Vector2Int[,] CurrentMatrix { get => currentMatrix; }
    public int PatternModsCount { get => patternModsCount; }

    public FieldController(string xmlDictionaryName, int SaveLevel, List<CardData> CardsDataList)
    {
        level = SaveLevel;
        cardsList = CardsDataList;
        fieldSize = (int)MathF.Sqrt(cardsList.Count);

        selectedWordsList = new List<string>();
        dictionary = new DictionaryController().GetDictionary(xmlDictionaryName);

        if (level * fieldSize < dictionary.Count)
        {
            InitializeCardsMatrix(fieldSize);
            SelectWords();
            FillPlayingField();
        }
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
        if (level * fieldSize >= dictionary.Count)
            return;
        
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
    private void FillPlayingField()
    {
        currentMatrix = GetFieldMatrix();
        int patternModIndex = level % patternModsCount;
        int x, y = 0;

        for (int i = 0; i < fieldSize; i++)
        {
            for (int j = 0; j < fieldSize; j++)
            {
                x = currentMatrix[i, j].x;
                y = currentMatrix[i, j].y;

                switch (patternModIndex)
                {
                    case 0:
                        cardsMatrix[x, y].textLetter.text = selectedWordsList[i][j].ToString();
                        break;
                    case 1:
                        cardsMatrix[y, fieldSize - x - 1].textLetter.text = selectedWordsList[i][j].ToString();
                        break;
                    case 2:
                        cardsMatrix[fieldSize - y - 1, x].textLetter.text = selectedWordsList[i][j].ToString();
                        break;
                    case 3:
                        cardsMatrix[fieldSize - x - 1, fieldSize - y - 1].textLetter.text = selectedWordsList[i][j].ToString();
                        break;
                    case 4:
                        cardsMatrix[x, fieldSize - y - 1].textLetter.text = selectedWordsList[i][j].ToString();
                        break;
                    case 5:
                        cardsMatrix[fieldSize - y - 1, fieldSize - x - 1].textLetter.text = selectedWordsList[i][j].ToString();
                        break;
                    case 6:
                        cardsMatrix[y, x].textLetter.text = selectedWordsList[i][j].ToString();
                        break;
                    case 7:
                        cardsMatrix[fieldSize - x - 1, y].textLetter.text = selectedWordsList[i][j].ToString();
                        break;
                }

                //Я бы куда нибудь это убрал
                cardsMatrix[i, j].textLetter.color = cardsMatrix[i, j].defoultColor;

                cardsMatrix[i, j].isHint = false;
            }
            Debug.Log(selectedWordsList[i]);
        }
    }

    private Vector2Int[,] GetFieldMatrix()
    {
        int patternIndex = 0;

        if(fieldSize == 3)
        {
            patternIndex = (level / patternModsCount) % Patterns.PatternsThreeLettersList.Count;
            return Patterns.PatternsThreeLettersList[patternIndex];
        }
        else if(fieldSize == 4)
        {
            patternIndex = (level / patternModsCount) % Patterns.PatternsFourLettersList.Count;
            return Patterns.PatternsFourLettersList[patternIndex];
        }

        else if (fieldSize == 5)
        {
            patternIndex = (level / patternModsCount) % Patterns.PatternsFiveLettersList.Count;
            return Patterns.PatternsFiveLettersList[patternIndex];
        }

        else
        {
            patternIndex = (level / patternModsCount) % Patterns.PatternsSixLettersList.Count;
            return Patterns.PatternsSixLettersList[patternIndex];
        }
    }

    public bool CheckLevelsComplete()
    {
        Debug.Log($"Level: {level * fieldSize}, DictCount: {dictionary.Count}");
        if (level * fieldSize >= dictionary.Count)
            return true;

        else
            return false;
    }

    public void RaiseLevel(int Value)
    {
        level += Value;
    }

    public void NewLevel()
    {
        selectedWordsList.Clear();

        SelectWords();
        FillPlayingField();

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
