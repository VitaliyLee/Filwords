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
    [SerializeField] private int level;//�� ������ �������� �� �������� ���������. ��� ����� �����������

    private int fieldSize;

    private List<CardData> cardsList;
    private Dictionary<string, string> dictionary; //���������� ��� �������

    private List<string> selectedWordsList;//������ ���� �� ������
    private CardData[,] cardsMatrix;//����� ��� �� �� cardsList ������� ������� ��� �������� ������

    Vector2Int[,] currentMatrix;

    private int patternModsCount = 8; //����������� ������������� ��������

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
        if (level > dictionary.Count - 1) level = 0;

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
