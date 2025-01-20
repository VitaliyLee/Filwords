using System;
using System.Collections.Generic;
using UnityEngine;

public class FieldController
{
    private int level;//�� ������ �������� �� �������� ���������. ��� ����� �����������
    private int fieldSize;

    private List<CardData> cardsList;
    private Dictionary<string, string> dictionary; //���������� ��� �������

    private List<string> selectedWordsList;//������ ���� �� ������
    private CardData[,] cardsMatrix;//����� ��� �� �� cardsList ������� ������� ��� �������� ������

    Vector2Int[,] currentMatrix;
    private int patternModsCount = 8; //����������� ������������� ��������

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

    //��� �� ������� ���� �������� � ���������� ����, ���������� �� � �������
    private void InitializeCardsMatrix(int fieldScale)
    {
        //������ ����� ������, ��� ����� ����������� ������ � �������� �� ���� 
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

    //�������� ����� ��� ������
    private void SelectWords()
    {
        if (level * fieldSize >= dictionary.Count)
            return;
        
        int wordIndex = level * fieldSize;
        string word = "";

        //�������� ����� �� ������ ���, ��� �� ��� 5 ���� �����������
        while (selectedWordsList.Count < fieldSize)
        {
            dictionary.TryGetValue(wordIndex.ToString(), out word);

            //���������� ����, ���� � ������ ��� ���� ����� �����
            if (!selectedWordsList.Contains(word))
                selectedWordsList.Add(word);

            wordIndex++;
        }
    }

    //���� ������ ���������� ������� � ������� � ��������
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

                //� �� ���� ������ ��� �����
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

    //����� ������� ����-���� 72

    //������� ���������

    //1. cardsMatrix[x, y].textLetter.text = selectedWordsList[i][j].ToString(); - ������� ���������� 

    //2. cardsMatrix[y, fieldScale - x - 1].textLetter.text = selectedWordsList[i][j].ToString(); - ������� ������� �� 90 �������� �� ������� �������

    //3. cardsMatrix[fieldScale - y - 1, x].textLetter.text = selectedWordsList[i][j].ToString(); - ������� ������� �� 90 �������� ������ ������� �������

    //4. cardsMatrix[fieldScale - x - 1, fieldScale - y - 1].textLetter.text = selectedWordsList[i][j].ToString(); - ������� ������� �� 180 ��������

    //��������� ��������� � ���������

    //5. cardsMatrix[x, fieldScale - y - 1].textLetter.text = selectedWordsList[i][j].ToString(); - ������������� �������

    //6. cardsMatrix[fieldScale - y - 1, fieldScale - x - 1].textLetter.text = selectedWordsList[i][j].ToString(); - ������������� � ������������ �� 90 �������� �� ������� �������

    //7. cardsMatrix[y, x].textLetter.text = selectedWordsList[i][j].ToString(); - ����������� �� 90 �������� ������ ������� ������� � ������������� �������

    //8. cardsMatrix[fieldScale - x - 1, y].textLetter.text = selectedWordsList[i][j].ToString(); - ������������ �� 180 �������� � ������������� �������


    //9. cardsMatrix[x, y].textLetter.text = selectedWordsList[i][fieldScale - j - 1].ToString(); - ������ ����� � ����� �������. ��� �� ������������� � ������� ������������� ����� �������� �� selectedWordsList[i][fieldScale - j - 1].ToString() ��������� �����
}
