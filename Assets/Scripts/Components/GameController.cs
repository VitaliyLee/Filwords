using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject messagePanel;

    [SerializeField] private GameObject letterTextObject;
    [SerializeField] private TextMeshProUGUI answerLetters;
    [SerializeField] private List<Color> colorsList;

    private FieldController fieldController;

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
        int row = 0;
        int x, y = 0;
        int patternModIndex = fieldController.Level % fieldController.PatternModsCount;
        Vector2Int fieldLetterPos = new Vector2Int();

        //Проверка правильности слова и его составления
        if (fieldController.SelectedWordsList.Contains(currentWord))
        {
            row = fieldController.SelectedWordsList.FindIndex(row => row.Contains(currentWord));//Находим индекс слова, тк позиция всех букв слова является СТРОКОЙ матрицы
            
            //Проверка на тоЮ что слово составлено из нужных ячеек
            for (int i = 0; i < selectedCardsList.Count; i++)
            {
                x = fieldController.CurrentMatrix[row, i].x;
                y = fieldController.CurrentMatrix[row, i].y;

                switch (patternModIndex)
                {
                    case 0:
                        fieldLetterPos.x = x; fieldLetterPos.y = y;
                        break;
                    case 1:
                        fieldLetterPos.x = y; fieldLetterPos.y = selectedCardsList.Count - x - 1;
                        break;
                    case 2:
                        fieldLetterPos.x = selectedCardsList.Count - y - 1; fieldLetterPos.y = x;
                        break;
                    case 3:
                        fieldLetterPos.x = selectedCardsList.Count - x - 1; fieldLetterPos.y = selectedCardsList.Count - y - 1;
                        break;
                    case 4:
                        fieldLetterPos.x = x; fieldLetterPos.y = selectedCardsList.Count - y - 1;
                        break;
                    case 5:
                        fieldLetterPos.x = selectedCardsList.Count - y - 1; fieldLetterPos.y = selectedCardsList.Count - x - 1;
                        break;
                    case 6:
                        fieldLetterPos.x = y; fieldLetterPos.y = x;
                        break;
                    case 7:
                        fieldLetterPos.x = selectedCardsList.Count - x - 1; fieldLetterPos.y = y;
                        break;
                }

                //Вывод сообщения если слово не соответствует паттерну
                if (fieldLetterPos != selectedCardsList[i].cardIndex)
                {
                    messagePanel.SetActive(true);
                    for (int j = 0; j < selectedCardsList.Count; j++)
                        selectedCardsList[j].image.color = Color.white;
                    return;
                }
            }

            for (int i = 0; i < selectedCardsList.Count; i++)
                disableCardsList.Add(selectedCardsList[i]);//Запись ячеек в список отгаданных
            currentColorIndex++;
        }

        else
            for (int i = 0; i < selectedCardsList.Count; i++)
                selectedCardsList[i].image.color = Color.white;

        //Если все слова угаданы
        if (disableCardsList.Count == (int)Mathf.Pow(fieldController.SelectedWordsList.Count, 2))
            winPanel.SetActive(true);
    }

    public void StartGame(LevelData levelData)
    {
        fieldController = new FieldController(levelData.DictionaryName, levelData.Level, levelData.CardsList);
    }

    public void NextLevel()
    {
        for (int i = 0; i < disableCardsList.Count; i++)
            disableCardsList[i].image.color = Color.white;
        disableCardsList.Clear();

        currentColorIndex = 0;

        fieldController.NewLevel();
    }

    public void BackFromLevel()
    {
        for (int i = 0; i < disableCardsList.Count; i++)
            disableCardsList[i].image.color = Color.white;
        disableCardsList.Clear();

        currentColorIndex = 0;
    }
}
