using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class FieldController : MonoBehaviour
{
    [SerializeField] private string xmlFileName; // ��� ����� ��� ����������
    [SerializeField] private int wordsCount;
    [SerializeField] private List<CardData> cardsList;

    private DictionaryController dictionaryController; //����� ����������� ���������
    private Dictionary<string, string> dictionary; //���������� ��� �������

    private List<string> selectedWordsList;//������ ���� �� ������
    private CardData[,] cardsMatrix;//����� ��� �� �� cardsList ������� ������� ��� �������� ������

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

    //����� ������� ����-����

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
