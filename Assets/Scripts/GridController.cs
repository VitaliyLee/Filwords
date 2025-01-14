using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] private List<CardData> cardsList;

    public List<CardData> CardsList { get => cardsList; }
}
