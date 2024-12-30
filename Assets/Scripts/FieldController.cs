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
    [SerializeField] private List<CardController> cardsList;

    private DictionaryController dictionaryController; //����� ����������� ���������
    private Dictionary<string, string> dictionary; //���������� ��� �������

    private List<string> selectedWordsList;//������ ���� �� ������
    private CardController[,] cardsMatrix;//����� ��� �� �� cardsList ������� ������� ��� �������� ������

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
