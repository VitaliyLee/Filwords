using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardData : MonoBehaviour
{
    public TextMeshProUGUI textLetter;
    public Image image;

    [HideInInspector]
    public Vector2Int cardIndex;

    public bool IsNeighbour(Vector2Int NeighbourIndex)
    {
        Vector2Int resultVector = NeighbourIndex - cardIndex;

        if(Mathf.Abs(resultVector.x) + Mathf.Abs(resultVector.y) == 1)
            return true;

        return false;
    }
}
