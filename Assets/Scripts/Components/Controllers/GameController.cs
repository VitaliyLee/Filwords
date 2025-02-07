using GamePush;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private AchievementSystem achievementSystem;
    [SerializeField] private LeaderBoard leaderBoard;
    [SerializeField] private SoundController soundController;
    [SerializeField] private WinDataController winPanel;
    [SerializeField] private GameObject difficultyLevelComplited;
    [SerializeField] private MessageController messageController;

    [SerializeField] private GameObject letterTextObject;
    [SerializeField] private TextMeshProUGUI answerLetters;
    [SerializeField] private TextMeshProUGUI gameTimeText;
    [SerializeField] private Color colorHint;
    [SerializeField] private List<Color> colorsList;

    //Ебать пиздец. Для одного туториала ....
    public delegate void FindingWordHendler();
    public event FindingWordHendler FindingWordEvent;

    private FieldController fieldController;

    private List<CardData> selectedCardsList;
    private List<CardData> disableCardsList;

    private string currentWord;
    private bool isDragging;

    private int currentColorIndex;//Прибавляется с каждым отгаданным словом, потом обнуляется
    private List<int> hintWordIndexList;

    private float gameTime;

    private void OnEnable() => GP_Init.OnReady += StartGame;
    private void OnDisable() => GP_Init.OnReady -= StartGame;

    private void StartGame()
    {
        currentColorIndex = 0;
        selectedCardsList = new List<CardData>();
        disableCardsList = new List<CardData>();
        hintWordIndexList = new List<int>();

        Saver.Load();//Загружает данные игрока с сервера
        player.SetGuessedWords();//Задает колличсетво угаданных слов на каждый уровень
        player.ViewResettingButton();
        player.AddScore(Saver.Score);
        achievementSystem.UpdateAchievement();
        leaderBoard.Fetch();
    }

    void Update()
    {
        if(fieldController != null)
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
                SelectCard();

            SetAnswerLetters();
            TimeCounter();
        }
    }

    private CardData IsPointerOverUIObject()
    {
        CardData cardController;
        // Получаем текущую позицию курсора
        PointerEventData pointerData = new PointerEventData(EventSystem.current){ position = Input.mousePosition };

        // Выполняем рейкаст
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        // Проверяем, есть ли наш целевой Button среди результатов
        foreach (RaycastResult result in results)
        {
            cardController = result.gameObject.GetComponent<CardData>();//Тут желательно тоже выбрать ДРУГОЕ решение, но пока пусть так

            if(cardController is CardData)
                return cardController;
        }

        return null;
    }

    private void SelectCard()
    {
        CardData cardController = IsPointerOverUIObject();

        if (!disableCardsList.Contains(cardController) && !selectedCardsList.Contains(cardController) && cardController != null)//disableCardsList - список не доступных ячеек (например потому что слова из них уже составлены)
        {
            if (selectedCardsList.Count > 0 && !cardController.IsNeighbour(selectedCardsList[selectedCardsList.Count - 1].cardIndex))//IsNeighbour проверяет является ли выбранная ячейка соседом последней ячейки из списка выделенных
                return;

            selectedCardsList.Add(cardController);//Добавляет в список выделенных объектов
            soundController.PlaySelectSoundWithRaisingTone(selectedCardsList.Count);

            cardController.image.color = colorsList[currentColorIndex];//Красит выбранный объект
            cardController.textLetter.color = Color.white;
            currentWord += cardController.textLetter.text;//Добавляет выбранную букву к результирубщему слову
        }

        //Проверка на "обратный ход". Если вести курсор обратно по слову, то ячейки перестают выделяться, дропаются из списка выбранных и из результирубщего слова
        if (selectedCardsList.Count > 1 && selectedCardsList[selectedCardsList.Count - 2] == cardController)
        {
            CardResetSettings(selectedCardsList.Count - 1);

            selectedCardsList.Remove(selectedCardsList[selectedCardsList.Count - 1]);
            currentWord = currentWord.Remove(currentWord.Length - 1);
        }
    }

    //Возвращает настройки карточки к изначальным
    private void CardResetSettings(int CardIndex)
    {
        if (selectedCardsList[CardIndex].isHint)
            selectedCardsList[CardIndex].textLetter.color = selectedCardsList[CardIndex].hintColor;
        else
            selectedCardsList[CardIndex].textLetter.color = selectedCardsList[CardIndex].defoultColor;

        selectedCardsList[CardIndex].image.color = Color.white;
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

        //Проверка правильности слова и его составления
        if (fieldController.SelectedWordsList.Contains(currentWord))
        {
            row = fieldController.SelectedWordsList.FindIndex(row => row.Contains(currentWord));//Находим индекс слова, тк позиция всех букв слова является СТРОКОЙ матрицы
            
            //Проверка на то что слово составлено из нужных ячеек
            for (int i = 0; i < selectedCardsList.Count; i++)
            {
                //Вывод сообщения если слово не соответствует паттерну
                if (GetLetterPos(row, i) != selectedCardsList[i].cardIndex)
                {
                    Saver.SaveAchieve("FirstMistake");
                    messageController.MessageText = "Попробуйте собрать слово по другому!";
                    messageController.gameObject.SetActive(true);

                    for (int j = 0; j < selectedCardsList.Count; j++)
                        CardResetSettings(j);
                    return;
                }
            }

            for (int i = 0; i < selectedCardsList.Count; i++)
                disableCardsList.Add(selectedCardsList[i]);//Запись ячеек в список отгаданных
            currentColorIndex++;
            FindingWordEvent?.Invoke();
            soundController.PlayTrueAnswernSound();

            hintWordIndexList.Remove(row);
        }
        
        else
            for (int i = 0; i < selectedCardsList.Count; i++)
                CardResetSettings(i);

        //Если все слова угаданы
        if (disableCardsList.Count == (int)Mathf.Pow(fieldController.SelectedWordsList.Count, 2))
        {
            if(!fieldController.CheckLevelsComplete())
                fieldController.RaiseLevel(1);//1 - потому что уровни прибавляются по одному

            if (fieldController.CheckLevelsComplete())
            {
                CompleteLevel(difficultyLevelComplited);
                return;
            }

            CompleteLevel(winPanel.gameObject);
            soundController.PlayWinSound();
        }
    }

    private void CompleteLevel(GameObject WinPanel)
    {
        Ad.ShowFullscreenAd((int)gameTime);
        int scoreTolevel = player.AddScoreForAnswer(disableCardsList.Count, (int)(gameTime / 60));

        WinPanel.SetActive(true);
        winPanel.SetEvaluation((int)gameTime, scoreTolevel, disableCardsList.Count);
        achievementSystem.CheckAchievement(disableCardsList.Count, fieldController.DictionaryLength, fieldController.Level, gameTime);

        ClearLevel();
        SaveLevelData();
    }

    private void TimeCounter()
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

    //Возвращает позицию карточки с искомой буквой, где row и column обознаяают индекс карточки в изначальном моссиве
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

    private void ClearLevel()
    {
        for (int i = 0; i < disableCardsList.Count; i++)
        {
            disableCardsList[i].textLetter.color = disableCardsList[i].defoultColor;
            disableCardsList[i].image.color = Color.white;
        }

        disableCardsList.Clear();
        hintWordIndexList.Clear();

        currentColorIndex = 0;
    }

    private void SaveLevelData()
    {
        //Может быть поменяю на другое, но вроде и так норм. коротко и понятно.
        switch (fieldController.CardsList.Count)
        {
            case 9:
                Saver.SaveWithScore("LevelNoob", fieldController.Level, player.Score);
                break;
            case 16:
                Saver.SaveWithScore("LevelNormal", fieldController.Level, player.Score);
                break;
            case 25:
                Saver.SaveWithScore("LevelVeteran", fieldController.Level, player.Score);
                break;
            case 36:
                Saver.SaveWithScore("LevelProfessionsl", fieldController.Level, player.Score);
                break;
        }
    }

    private void GetHint()
    {
        if (hintWordIndexList.Count <= 0)
        {
            messageController.MessageText = "Вы получили все подсказки!";
            messageController.gameObject.SetActive(true);
            return;
        }

        Vector2Int fieldLetterPos = GetLetterPos(hintWordIndexList[0], 0);//У hintWordIndexList индекс 0 потому что в целом без разницы какой элемент подсказывать игроку

        for (int i = 0; i < fieldController.CardsList.Count; i++)
        {
            if (fieldController.CardsList[i].cardIndex == fieldLetterPos)//Поиск карточки с нужной буквой по её позиции с учетом поворота
            {
                fieldController.CardsList[i].isHint = true;
                fieldController.CardsList[i].textLetter.color = fieldController.CardsList[i].hintColor;
                hintWordIndexList.RemoveAt(0);
            }
        }

        Saver.SaveAchieve("20");//Очень-очень плохо, но и архитектура у меня КАЛ
        soundController.PlayHintSound();
    }

    public void StartGame(LevelData levelData)
    {
        fieldController = new FieldController(levelData.DictionaryName, levelData.Level, levelData.CardsList);

        if (fieldController.CheckLevelsComplete())
        {
            difficultyLevelComplited.SetActive(true);

            return;
        }

        for (int i = 0; i < fieldController.SelectedWordsList.Count; i++)
            hintWordIndexList.Add(i);

        gameTime = 0;
    }

    public void NextLevel()
    {
        gameTime = 0;
        fieldController.NewLevel();

        for (int i = 0; i < fieldController.SelectedWordsList.Count; i++)
            hintWordIndexList.Add(i);

        player.SetGuessedWords();
        player.ViewResettingButton();
    }

    public void BackFromLevel()
    {
        fieldController = null;

        player.SetGuessedWords();
        player.ViewResettingButton();
        ClearLevel();
    }

    public void ShowRewarded() => Ad.ShowRewardedAd(() => GetHint());
}
