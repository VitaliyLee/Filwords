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

    private int currentColorIndex;//Прибавляется с каждым отгаданным словом, потом обнуляется
    
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

            if (!disableCardsList.Contains(cardController) && !selectedCardsList.Contains(cardController) && cardController != null)//disableCardsList - список не доступных ячеек (например потому что слова из них уже составлены)
            {
                if (selectedCardsList.Count > 0 && !cardController.IsNeighbour(selectedCardsList[selectedCardsList.Count - 1].cardIndex))//IsNeighbour проверяет является ли выбранная ячейка соседом последней ячейки из списка выделенных
                    return false;

                selectedCardsList.Add(cardController);//Добавляет в список выделенных объектов
                cardController.image.color = colorsList[currentColorIndex];//Красит выбранный объект
                currentWord += cardController.textLetter.text;//Добавляет выбранную букву к результирубщему слову

                return true;
            }

            //Проверка на "обратный ход". Если вести курсор обратно по слову, то ячейки перестают выделяться, дропаются из списка выбранных и из результирубщего слова
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
        //Управление текстом ответа
        if (answerLetters.text != "")
            letterTextObject.SetActive(true);
        else
            letterTextObject.SetActive(false);

        answerLetters.text = currentWord;//Потом придумать что нибудь ПОЛУЧШЕ!!!
    }

    private void CheckCorrectnessWord()
    {
        //Если слово угадано
        if (fieldController.SelectedWordsList.Contains(currentWord))
        {
            for (int i = 0; i < selectedCardsList.Count; i++)
                disableCardsList.Add(selectedCardsList[i]);//Запись ячеек в список отгаданных
            currentColorIndex++;
        }

        else
            for (int i = 0; i < selectedCardsList.Count; i++)
                selectedCardsList[i].image.color = Color.white;

        //Если все слова угаданы
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
