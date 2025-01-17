using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private MessageController messageController;

    [SerializeField] private GameObject letterTextObject;
    [SerializeField] private TextMeshProUGUI answerLetters;
    [SerializeField] private TextMeshProUGUI gameTimeText;
    [SerializeField] private Color colorHint;
    [SerializeField] private List<Color> colorsList;

    private FieldController fieldController;

    private List<CardData> selectedCardsList;
    private List<CardData> disableCardsList;

    private string currentWord;
    private bool isDragging;

    private int currentColorIndex;//������������ � ������ ���������� ������, ����� ����������
    private List<int> hintWordIndexList;

    private float gameTime;
    private int minutes, seconds;

    private void Start()
    {
        currentColorIndex = 0;
        selectedCardsList = new List<CardData>();
        disableCardsList = new List<CardData>();
        hintWordIndexList = new List<int>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if(fieldController != null)
            {
                CheckCorrectnessWord();

                currentWord = "";
                isDragging = false;
                selectedCardsList.Clear();
            }
        }
        
        if (isDragging)
            IsPointerOverUIObject();

        SetAnswerLetters();
        Timer();
    }

    private bool IsPointerOverUIObject()
    {
        // �������� ������� ������� �������
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        // ������ ��� �������� �����������
        var results = new List<RaycastResult>();

        // ��������� �������
        EventSystem.current.RaycastAll(pointerData, results);

        // ���������, ���� �� ��� ������� Button ����� �����������
        foreach (RaycastResult result in results)
        {
            CardData cardController = result.gameObject.GetComponent<CardData>();//��� ���������� ���� ������� ������ �������, �� ���� ����� ���

            if (!disableCardsList.Contains(cardController) && !selectedCardsList.Contains(cardController) && cardController != null)//disableCardsList - ������ �� ��������� ����� (�������� ������ ��� ����� �� ��� ��� ����������)
            {
                if (selectedCardsList.Count > 0 && !cardController.IsNeighbour(selectedCardsList[selectedCardsList.Count - 1].cardIndex))//IsNeighbour ��������� �������� �� ��������� ������ ������� ��������� ������ �� ������ ����������
                    return false;

                selectedCardsList.Add(cardController);//��������� � ������ ���������� ��������
                cardController.image.color = colorsList[currentColorIndex];//������ ��������� ������
                cardController.textLetter.color = Color.white;
                currentWord += cardController.textLetter.text;//��������� ��������� ����� � ��������������� �����

                return true;
            }

            //�������� �� "�������� ���". ���� ����� ������ ������� �� �����, �� ������ ��������� ����������, ��������� �� ������ ��������� � �� ��������������� �����
            if (selectedCardsList.Count > 1 && selectedCardsList[selectedCardsList.Count - 2] == cardController)
            {
                selectedCardsList[selectedCardsList.Count - 1].image.color = Color.white;
                selectedCardsList[selectedCardsList.Count - 1].textLetter.color = selectedCardsList[selectedCardsList.Count - 1].defoultColor;
                selectedCardsList.Remove(selectedCardsList[selectedCardsList.Count - 1]);
                currentWord = currentWord.Remove(currentWord.Length - 1);
            }
        }

        return false;
    }

    private void SetAnswerLetters()
    {
        //���������� ������� ������
        if (answerLetters.text != "")
            letterTextObject.SetActive(true);
        else
            letterTextObject.SetActive(false);

        answerLetters.text = currentWord;//����� ��������� ��� ������ �������!!!
    }

    private void CheckCorrectnessWord()
    {
        int row = 0;

        //�������� ������������ ����� � ��� �����������
        if (fieldController.SelectedWordsList.Contains(currentWord))
        {
            row = fieldController.SelectedWordsList.FindIndex(row => row.Contains(currentWord));//������� ������ �����, �� ������� ���� ���� ����� �������� ������� �������
            
            //�������� �� ��� ��� ����� ���������� �� ������ �����
            for (int i = 0; i < selectedCardsList.Count; i++)
            {
                //����� ��������� ���� ����� �� ������������� ��������
                if (GetLetterPos(row, i) != selectedCardsList[i].cardIndex)
                {
                    messageController.MessageText = "���������� ������� ����� �� �������!";
                    messageController.gameObject.SetActive(true);

                    for (int j = 0; j < selectedCardsList.Count; j++)
                    {
                        if (selectedCardsList[j].isHint)
                            selectedCardsList[j].textLetter.color = selectedCardsList[j].hintColor;
                        else
                            selectedCardsList[j].textLetter.color = selectedCardsList[j].defoultColor;

                        selectedCardsList[j].image.color = Color.white;
                    }
                    return;
                }
            }

            for (int i = 0; i < selectedCardsList.Count; i++)
                disableCardsList.Add(selectedCardsList[i]);//������ ����� � ������ ����������
            currentColorIndex++;

            hintWordIndexList.Remove(row);
        }
        
        else
            for (int i = 0; i < selectedCardsList.Count; i++)
            {
                if(selectedCardsList[i].isHint)
                    selectedCardsList[i].textLetter.color = selectedCardsList[i].hintColor;
                else
                    selectedCardsList[i].textLetter.color = selectedCardsList[i].defoultColor;

                selectedCardsList[i].image.color = Color.white;
            }

        //���� ��� ����� �������
        if (disableCardsList.Count == (int)Mathf.Pow(fieldController.SelectedWordsList.Count, 2))
        {
            winPanel.SetActive(true);
            player.AddScoreForAnswer(disableCardsList.Count, (int)(gameTime / 60));
        }
    }

    private void Timer()
    {
        gameTime += Time.deltaTime;
        int min = (int)(gameTime / 60);
        int sec = (int)(gameTime % 60);

        if(min < 10 && sec < 10)
            gameTimeText.text = $"0{min} : 0{sec}"; 
        else if (sec < 10)
            gameTimeText.text = $"{min} : 0{sec}";
        else if(min < 10)
            gameTimeText.text = $"0{min} : {sec}";
        else
            gameTimeText.text = $"{min} : {sec}";
    }

    //���������� ������� �������� � ������� ������, ��� row � column ���������� ������ �������� � ����������� �������
    private Vector2Int GetLetterPos(int row, int column)
    {
        int x, y = 0;
        int patternModIndex = fieldController.Level % fieldController.PatternModsCount;
        Vector2Int fieldLetterPos = new Vector2Int();
        
        x = fieldController.CurrentMatrix[row, column].x;
        y = fieldController.CurrentMatrix[row, column].y;

        switch (patternModIndex)
        {
            case 0:
                fieldLetterPos.x = x; fieldLetterPos.y = y;
                break;
            case 1:
                fieldLetterPos.x = y; fieldLetterPos.y = fieldController.SelectedWordsList.Count - x - 1;
                break;
            case 2:
                fieldLetterPos.x = fieldController.SelectedWordsList.Count - y - 1; fieldLetterPos.y = x;
                break;
            case 3:
                fieldLetterPos.x = fieldController.SelectedWordsList.Count - x - 1; fieldLetterPos.y = fieldController.SelectedWordsList.Count - y - 1;
                break;
            case 4:
                fieldLetterPos.x = x; fieldLetterPos.y = fieldController.SelectedWordsList.Count - y - 1;
                break;
            case 5:
                fieldLetterPos.x = fieldController.SelectedWordsList.Count - y - 1; fieldLetterPos.y = fieldController.SelectedWordsList.Count - x - 1;
                break;
            case 6:
                fieldLetterPos.x = y; fieldLetterPos.y = x;
                break;
            case 7:
                fieldLetterPos.x = fieldController.SelectedWordsList.Count - x - 1; fieldLetterPos.y = y;
                break;
        }

        return fieldLetterPos;
    }

    public void StartGame(LevelData levelData)
    {
        fieldController = new FieldController(levelData.DictionaryName, levelData.Level, levelData.CardsList);

        for (int i = 0; i < fieldController.SelectedWordsList.Count; i++)
            hintWordIndexList.Add(i);

        gameTime = 0;
    }

    public void GetHint()
    {
        if (hintWordIndexList.Count <= 0)
        {
            messageController.MessageText = "�� �������� ��� ���������!";
            messageController.gameObject.SetActive(true);
            return;
        }

        Vector2Int fieldLetterPos = GetLetterPos(hintWordIndexList[0], 0);//� hintWordIndexList ������ 0 ������ ��� � ����� ��� ������� ����� ������� ������������ ������

        for (int i = 0; i < fieldController.CardsList.Count; i++)
        {
            if (fieldController.CardsList[i].cardIndex == fieldLetterPos)//����� �������� � ������ ������ �� � ������� � ������ ��������
            {
                fieldController.CardsList[i].isHint = true;
                fieldController.CardsList[i].textLetter.color = fieldController.CardsList[i].hintColor;
                hintWordIndexList.RemoveAt(0);
            }
        }

        player.SubtractScore(25);
    }

    public void NextLevel()
    {
        for (int i = 0; i < disableCardsList.Count; i++)
            disableCardsList[i].image.color = Color.white;
        disableCardsList.Clear();

        hintWordIndexList.Clear();
        for (int i = 0; i < fieldController.SelectedWordsList.Count; i++)
            hintWordIndexList.Add(i);

        currentColorIndex = 0;
        gameTime = 0;

        fieldController.NewLevel();
    }

    public void BackFromLevel()
    {
        for (int i = 0; i < disableCardsList.Count; i++)
            disableCardsList[i].image.color = Color.white;
        disableCardsList.Clear();
        hintWordIndexList.Clear();

        fieldController = null;

        currentColorIndex = 0;
    }
}
