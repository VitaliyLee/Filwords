using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    public TextMeshProUGUI textLetter;

    private Image image;
    private bool isDragging;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            isDragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging) 
        {
            if (IsPointerOverUIObject())
            {
                image.color = Color.red; // ��������� ���������
            }
        }
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
            if (result.gameObject == gameObject)
                return true;

        return false;
    }
}
