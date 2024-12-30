using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    public TextMeshProUGUI textLetter;
    public Image image;

    [HideInInspector]
    public Vector2Int cardIndex;
}
