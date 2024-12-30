using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject letterTextObject;
    [SerializeField] private TextMeshProUGUI answerLetters;

    private List<CardData> selectedCardsList;
    private string currentWord;
    private bool isDragging;

    private void Start()
    {
        selectedCardsList = new List<CardData>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            currentWord = "";
            isDragging = false;

            for (int i = 0; i < selectedCardsList.Count; i++)
                selectedCardsList[i].image.color = Color.white;

            selectedCardsList.Clear();
        }

        if (isDragging)
        {
            if (IsPointerOverUIObject())
            {
                selectedCardsList[selectedCardsList.Count - 1].image.color = Color.red; // Применяем изменения
            }
        }

        if(answerLetters.text != "")
            letterTextObject.SetActive(true);
        else
            letterTextObject.SetActive(false);

        answerLetters.text = currentWord;//Потом придумать что нибудь ПОЛУЧШЕ!!!
    }

    private bool IsPointerOverUIObject()
    {
        // Получаем текущую позицию курсора
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        // Список для хранения результатов
        var results = new List<RaycastResult>();

        // Выполняем рейкаст
        EventSystem.current.RaycastAll(pointerData, results);

        // Проверяем, есть ли наш целевой Button среди результатов
        foreach (RaycastResult result in results)
        {
            CardData cardController = result.gameObject.GetComponent<CardData>();//Тут желательно тоже выбрать ДРУГОЕ решение, но пока пусть так

            if (!selectedCardsList.Contains(cardController) && cardController != null)
            {
                selectedCardsList.Add(cardController);
                currentWord += cardController.textLetter.text;

                return true;
            }
        }

        return false;
    }
}
