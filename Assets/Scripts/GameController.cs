using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;

    [SerializeField] private FieldController fieldController;
    [SerializeField] private GameObject letterTextObject;
    [SerializeField] private TextMeshProUGUI answerLetters;
    [SerializeField] private List<Color> colorsList;

    private List<CardData> selectedCardsList;
    private List<CardData> disableCardsList;

    private string currentWord;
    private bool isDragging;

    private int currentColorIndex;//������������ � ������ ���������� ������, ����� ����������
    
    private void Start()
    {
        currentColorIndex = 0;
        selectedCardsList = new List<CardData>();
        disableCardsList = new List<CardData>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            CheckCorrectnessWord();

            currentWord = "";
            isDragging = false;
            selectedCardsList.Clear();
        }
        
        if (isDragging)
            IsPointerOverUIObject();

        SetAnswerLetters();
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
                currentWord += cardController.textLetter.text;//��������� ��������� ����� � ��������������� �����

                return true;
            }

            //�������� �� "�������� ���". ���� ����� ������ ������� �� �����, �� ������ ��������� ����������, ��������� �� ������ ��������� � �� ��������������� �����
            if (selectedCardsList.Count > 1 && selectedCardsList[selectedCardsList.Count - 2] == cardController)
            {
                selectedCardsList[selectedCardsList.Count - 1].image.color = Color.white;
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
        //���� ����� �������
        if (fieldController.SelectedWordsList.Contains(currentWord))
        {
            for (int i = 0; i < selectedCardsList.Count; i++)
                disableCardsList.Add(selectedCardsList[i]);//������ ����� � ������ ����������
            currentColorIndex++;
        }

        else
            for (int i = 0; i < selectedCardsList.Count; i++)
                selectedCardsList[i].image.color = Color.white;

        //���� ��� ����� �������
        if (disableCardsList.Count == fieldController.FieldSize * fieldController.FieldSize)
            winPanel.SetActive(true);
    }

    public void NextLevel()
    {
        for (int i = 0; i < disableCardsList.Count; i++)
            disableCardsList[i].image.color = Color.white;
        disableCardsList.Clear();

        currentColorIndex = 0;

        fieldController.NewLevel();
    }
}
