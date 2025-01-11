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
    [SerializeField] private int startLevel;//�� ������ �������� �� �������� ���������. ��� ����� �����������

    [SerializeField] private string xmlFileName; // ��� ����� ��� ����������
    [SerializeField] private int wordsCount;
    [SerializeField] private int fieldSize;
    [SerializeField] private List<CardData> cardsList;

    private DictionaryController dictionaryController; //����� ����������� ���������
    private Dictionary<string, string> dictionary; //���������� ��� �������

    private List<string> selectedWordsList;//������ ���� �� ������
    private CardData[,] cardsMatrix;//����� ��� �� �� cardsList ������� ������� ��� �������� ������

    private int patternModsCount = 8; //����������� ������������� ��������

    public List<string> SelectedWordsList { get => selectedWordsList; }
    public int FieldSize { get => fieldSize; }

    private void Start()
    {
        selectedWordsList = new List<string>();
        dictionaryController = new DictionaryController();
        dictionary = dictionaryController.GetDictionary(xmlFileName);

        InitializeCardsMatrix(fieldSize);
        SelectWords();
        FillPlayingField(fieldSize, startLevel);
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
        int wordIndex = 0;
        string word = "";

        //�������� ����� �� ������ ���, ��� �� ��� 5 ���� �����������
        while (selectedWordsList.Count < wordsCount)
        {
            wordIndex = Random.Range(0, dictionary.Count);
            word = dictionaryController.GetTextById(wordIndex.ToString());

            //���������� ����, ���� � ������ ��� ���� ����� �����
            if (!selectedWordsList.Contains(word))
                selectedWordsList.Add(word);
        }
    }

    //���� ������ ���������� ������� � ������� � ��������
    private void FillPlayingField(int fieldScale, int level)
    {
        //����������� ������������� ��������� = 8
        int x, y = 0;

        int patternIndex = 7;
        int patternModIndex = level % patternModsCount;

        Vector2Int[,] currentMatrix = Patterns.PatternsList[patternIndex];

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

    public void NewLevel()
    {
        startLevel++;
        selectedWordsList.Clear();

        SelectWords();
        FillPlayingField(fieldSize, startLevel);
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
