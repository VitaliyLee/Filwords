using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    [SerializeField] private FieldController fieldController;

    private List<CardController> selectedCardsList;

    private bool isDragging;

    private void Start()
    {
        selectedCardsList = new List<CardController>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
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
            CardController cardController = result.gameObject.GetComponent<CardController>();

            if (cardController)
            {
                selectedCardsList.Add(cardController);
                return true;
            }
        }

        return false;
    }
}
